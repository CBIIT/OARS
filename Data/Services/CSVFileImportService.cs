using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using MimeKit;
using Okta.Sdk.Model;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

public class CSVFileImportService : ICSVFileImportService
{
    private readonly IProtocolEDCFormService _formService;
    private readonly IProtocolEDCFieldService _fieldService;
    private readonly IProtocolEDCDictionaryService _dictionaryService;

    public CSVFileImportService(IProtocolEDCFormService formService, IProtocolEDCFieldService fieldService, IProtocolEDCDictionaryService dictionaryService)
    { 
        _formService = formService;
        _fieldService = fieldService;
        _dictionaryService = dictionaryService;
	}

    public class CSVForm
    {
        [Name("Study Revision")]
        public int studyRevision { get; set; }

        [Name("Visit Id")]
        public int VisitId { get; set; }

        [Name("Visit Name")]
        public string visitName { get; set; }

        [Name("Visit Abbreviation")]
        public string visitAbbr { get; set; }

        [Name("Visit Add When")]
        public string visitAddWhen { get; set; }

        [Name("Visit Repeats")]
        public string visitRepeats { get; set; }

        [Name("Visit Max Repeats")]
        public string visitMaxRepeats { get; set; }

        [Name("Visit Object Name")]
        public string visitObjectName { get; set; }

        [Name("Order of Visit in Revision Visit Schedule")]
        public int visitOrderRevision { get; set; }

        [Name("Page Id")]
        public int pageId { get; set; }

        [Name("Page Name")]
        public string pageName { get; set; }

        [Name("Page Repeats")]
        public string pageRepeats { get; set; }

        [Name("Page Max Repeats")]
        public string pageMaxRepeats { get; set; }

        [Name("Page Object Name")]
        public string pageObjectName { get; set; }

        [Name("Page Added By Rule")]
        public string pageAddedByRule { get; set; }

        [Name("Order of Page in Visit Definition")]
        public int OrderPageVisitDef { get; set; }

    }

    public class CSVField
    {
        [Name("Page Object Id")]
        public int pageObjectId { get; set; }

        [Name("Page Name")]
        public string pageName { get; set; }

        [Name("Display Order on Page")]
        public int displayOrderOnPage { get; set; }

        [Name("Control Id")]
        public string controlId { get; set; }

        [Name("Control Type")]
        public string controlType { get; set; }

        [Name("Question Text")]
        public string questionText { get; set; }

        [Name("Codelist Name")]
        public string codelistName { get; set; }

        [Name("Digits Before Decimal")]
        public string digitsBeforeDecimal { get; set; }

        [Name("Digits After Decimal")]
        public string digitsAfterDecimal { get; set; }

        [Name("Maximum Text Length")]
        public string maxTextLength { get; set; }

        [Name("SDV")]
        public string sdv { get; set; }

        [Name("Export Table")]
        public string exportTable { get; set; }

        [Name("Export Column")]
        public string exportColumn { get; set; }

        [Name("Field Requires Medical Coding")]
        public string fieldRequiresMedicalCoding { get; set; }

        [Name("Medical Coding Dictionary Name")]
        public string medicalCodingDictionaryName { get; set; }

        [Name("Field Supports Empty State")]
        public string fieldSupportsEmptyState { get; set; }

    }

    public class CSVDictionary
    {
        [Name("Codelist name")]
        public string codeListName { get; set; }

        [Name("Codelist description")]
        public string codelistDescription { get; set; }

        [Name("Coded value")]
        public string codedValue { get; set; }

        [Name("Displayed value")]
        public string displayedValue { get; set; }

        [Name("Indicates that a value is hidden")]
        public string indicatesValueHidden { get; set; }

        [Name("Display order within codelist")]
        public string displayOrder { get; set; }

    }

    public async Task<List<string>> ParseCSVField(MemoryStream inputFileStream, int protocolMappingId)
    {
        var processInfo = new List<string>();

        var forms = await _formService.GetProtocolEDCFormsByProtocolMappingId(protocolMappingId);

        var skipped = 0;

        if (forms.Count > 0)
        {
            var formMap = forms.Where(x => x.EDCFormIdentifier != null).GroupBy(x => x.EDCFormIdentifier!).ToDictionary(x => x.Key, x => x.ToArray()[0]);
            DataTable fields = SetUpFieldTable();

            var reader = new StreamReader(inputFileStream, Encoding.UTF8);

            inputFileStream.Seek(0, SeekOrigin.Begin);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            var rowsSkipped = 0;
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<CSVField>().ToArray();

                processInfo.Add("Field records in file: " + records.Length.ToString());

                foreach (var record in records)
                {

                    if ((!string.IsNullOrEmpty(record.pageName) || !string.IsNullOrEmpty(record.exportColumn)))
                    {
                        var EDCForm = formMap.GetValueOrDefault(record.pageName);
                        if (EDCForm == null)
                            throw new Exception($"The form identifier {record.pageName} was not found in the list of forms");

                        DataRow field = fields.NewRow();
                        field["Update_Date"] = DateTime.Now;
                        field["Create_Date"] = DateTime.Now;
                        field["Protocol_EDC_Form_Id"] = EDCForm.ProtocolEDCFormId;
                        field["EDC_Field_Identifier"] = record.pageName;
                        field["EDC_Field_Name"] = record.questionText;
                        field["EDC_Dictionary_Name"] = record.codelistName;

                        fields.Rows.Add(field);
                    }
                    else
                    {
                        skipped++;
                    }
                }
            }

            if (fields.Rows.Count > 0)
            {
                bool saveFieldResult = await _fieldService.BulkSaveFields(fields);
                if (!saveFieldResult)
                {
                    throw new Exception("Error uploading Fields file, clear uploads and try again.");
                }
            }

            processInfo.Add("Field records inserted: " + fields.Rows.Count.ToString());
            processInfo.Add("Field records skipped: " + skipped.ToString());
        }
        else
        {
            processInfo.Add("No forms found in the database for the fields");
        }

        return processInfo;
    }

    public async Task<List<string>> ParseCSVFileForm(MemoryStream inputFileStream, int protocolMappingId)
    {
        var processInfo = new List<string>();

        List<ProtocolEDCForm> forms = new List<ProtocolEDCForm>();

        var reader = new StreamReader(inputFileStream, Encoding.UTF8);

        inputFileStream.Seek(0, SeekOrigin.Begin);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<CSVForm>().ToArray();
            
            processInfo.Add("Form records in file: " + records.Length.ToString());

            foreach (var record in records)
                {
                ProtocolEDCForm form = new ProtocolEDCForm
                    {
                        ProtocolMappingId = protocolMappingId,
                        EDCFormIdentifier = record.pageObjectName,
                        EDCFormName = record.pageName,
                        CreateDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
            
                forms.Add(form);
                }
        }

        if (forms.Count == 0)
        {
            throw new Exception("No forms found in the file.");
        }
        
        bool saveFormResult = await _formService.BulkSaveForms(forms);
        if (!saveFormResult)
        {
            throw new Exception("Error uploading file, clear uploads and try again.");
        }

        processInfo.Add("Form records inserted: " + forms.Count.ToString());

        return processInfo;
    }

    public async Task<List<string>> ParseCSVFileMeta(MemoryStream inputFileStream, int protocolMappingId)
    {
        var processInfo = new List<string>();

        DataTable dictionaries = SetUpDictionaryTable();

        var reader = new StreamReader(inputFileStream, Encoding.UTF8);

        inputFileStream.Seek(0, SeekOrigin.Begin);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<CSVDictionary>().ToArray();

            processInfo.Add("Dictionary records in file: " + records.Length.ToString());

            foreach (var record in records)
            {
                if (!string.IsNullOrEmpty(record.codeListName))
                {
                    DataRow dictionary = dictionaries.NewRow();
                    dictionary["Updated_Date"] = DateTime.Now;
                    dictionary["Create_Date"] = DateTime.Now;
                    dictionary["Protocol_Mapping_Id"] = protocolMappingId;
                    dictionary["EDC_Item_Id"] = record.codedValue;
                    dictionary["EDC_Item_Name"] = record.displayedValue;
                    dictionary["EDC_Dictionary_Name"] = record.codeListName;

                    dictionaries.Rows.Add(dictionary);
                }
            }
        }

        if (dictionaries.Rows.Count > 0)
        {
            bool saveDictResult = await _dictionaryService.BulkSaveDictionaries(dictionaries);
            if (!saveDictResult)
            {
                throw new Exception("Error uploading dictionary file, clear uploads and try again.");
            }
            Console.WriteLine($"Dictionaries saved: {dictionaries.Rows.Count}");
        }

        processInfo.Add("Dictonary records inserted: " + dictionaries.Rows.Count.ToString());

        return processInfo;
    }

    private DataTable SetUpDictionaryTable()
    {
        DataTable dictionaries = new DataTable();
        dictionaries.Columns.Add("Protocol_EDC_Dictionary_Id", typeof(int));
        dictionaries.Columns.Add("Protocol_Mapping_Id", typeof(int));
        dictionaries.Columns.Add("EDC_Item_Id", typeof(string));
        dictionaries.Columns.Add("EDC_Item_Name", typeof(string));
        dictionaries.Columns.Add("EDC_Dictionary_Name", typeof(string));
        dictionaries.Columns.Add("Create_Date", typeof(DateTime));
        dictionaries.Columns.Add("Updated_Date", typeof(DateTime));
        return dictionaries;
    }

    private DataTable SetUpFieldTable()
    {
        DataTable fields = new DataTable();
        fields.Columns.Add("Protocol_EDC_Field_Id", typeof(int));
        fields.Columns.Add("Protocol_EDC_Form_Id", typeof(int));
        fields.Columns.Add("EDC_Field_Identifier", typeof(string));
        fields.Columns.Add("EDC_Field_Name", typeof(string));
        fields.Columns.Add("EDC_Dictionary_Name", typeof(string));
        fields.Columns.Add("Create_Date", typeof(DateTime));
        fields.Columns.Add("Update_Date", typeof(DateTime));
        return fields;
    }
}
