﻿using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Data;
using System.Globalization;
using System.Text;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

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
        [Name("REVNUM", "Study Revision")]
        public int studyRevision { get; set; }

        [Name("VISITID", "Visit Id")]
        public int VisitId { get; set; }

        [Name("VISNAME", "Visit Name")]
        public string visitName { get; set; }

        [Name("VABBREV", "Visit Abbreviation")]
        public string visitAbbr { get; set; }

        [Name("VADDWHEN", "Visit Add When")]
        public string visitAddWhen { get; set; }

        [Name("VREPEATS", "Visit Repeats")]
        public string visitRepeats { get; set; }

        [Name("VREPEATSMAX", "Visit Max Repeats")]
        public string visitMaxRepeats { get; set; }

        [Name("VOBJNAME", "Visit Object Name")]
        public string visitObjectName { get; set; }

        [Name("VORDER", "Order of Visit in Revision Visit Schedule")]
        public int visitOrderRevision { get; set; }

        [Name("PAGEID", "Page Id")]
        public int pageId { get; set; }

        [Name("PAGENAME", "Page Name")]
        public string pageName { get; set; }

        [Name("PREPEATS", "Page Repeats")]
        public string pageRepeats { get; set; }

        [Name("PREPEATSMAX", "Page Max Repeats")]
        public string pageMaxRepeats { get; set; }

        [Name("POBJNAME", "Page Object Name")]
        public string pageObjectName { get; set; }

        [Name("PADDBYRULE", "Page Added By Rule")]
        public string pageAddedByRule { get; set; }

        [Name("PORDER", "Order of Page in Visit Definition")]
        public int OrderPageVisitDef { get; set; }

        [Name("Table Name")]
        public string tableName { get; set; }

    }

    public class CSVField
    {
        [Name("PAGEOBJECTID", "Page Object Id")]
        public int pageObjectId { get; set; }

        [Name("PAGENAME", "Page Name")]
        public string pageName { get; set; }

        [Name("DORDER", "Display Order on Page")]
        public int displayOrderOnPage { get; set; }

        [Name("CONTROLID", "Control Id")]
        public string controlId { get; set; }

        [Name("CONTROLTYPE", "Control Type")]
        public string controlType { get; set; }

        [Name("QUESTEXT", "Question Text")]
        public string questionText { get; set; }

        [Name("CODELISTNAME", "Codelist Name")]
        public string codelistName { get; set; }

        [Name("BEFOREDEC", "Digits Before Decimal")]
        public string digitsBeforeDecimal { get; set; }

        [Name("AFTERDEC", "Digits After Decimal")]
        public string digitsAfterDecimal { get; set; }

        [Name("MAXLENGTH", "Maximum Text Length")]
        public string maxTextLength { get; set; }

        [Name("SDV")]
        public string sdv { get; set; }

        [Name("REPORTINGT", "Export Table")]
        public string exportTable { get; set; }

        [Name("REPORTINGC", "Export Column")]
        public string exportColumn { get; set; }

        [Name("MEDCODING", "Field Requires Medical Coding")]
        public string fieldRequiresMedicalCoding { get; set; }

        [Name("DICTIONARYNAME", "Medical Coding Dictionary Name")]
        public string medicalCodingDictionaryName { get; set; }

        [Name("EMPTYSTATE", "Field Supports Empty State")]
        public string fieldSupportsEmptyState { get; set; }

    }

    public class CSVDictionary
    {
        [Name("NAME ", "Codelist name")]
        public string codeListName { get; set; }

        [Name("DESCRIPTION", "Codelist description")]
        public string codelistDescription { get; set; }

        [Name("CODENAME", "Coded value")]
        public string codedValue { get; set; }

        [Name("DISPLAYNAME", "Displayed value")]
        public string displayedValue { get; set; }

        [Name("HIDDEN", "Indicates that a value is hidden")]
        public string indicatesValueHidden { get; set; }

        [Name("DORDER", "Display order within codelist")]
        public string displayOrder { get; set; }

    }

    public async Task<List<string>> ParseCSVField(MemoryStream inputFileStream, int protocolMappingId)
    {
        var processInfo = new List<string>();

        var forms = await _formService.GetProtocolEDCFormsByProtocolMappingId(protocolMappingId);

        var skipped = 0;

        processInfo.Add($"Adding fields to {forms.Count.ToString()} forms");

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

                    if ((!string.IsNullOrEmpty(record.exportTable) || !string.IsNullOrEmpty(record.exportColumn)))
                    {
                        var EDCForm = formMap.GetValueOrDefault(record.pageName);
                        //if (EDCForm == null)
                        //    throw new Exception($"The form identifier {record.exportTable} was not found in the list of forms");
                        if (EDCForm != null)
                        {
                            DataRow field = fields.NewRow();
                            field["Update_Date"] = DateTime.Now;
                            field["Create_Date"] = DateTime.Now;
                            field["Protocol_EDC_Form_Id"] = EDCForm.ProtocolEDCFormId;
                            field["EDC_Field_Identifier"] = record.exportColumn;
                            field["EDC_Field_Name"] = record.questionText;
                            field["EDC_Dictionary_Name"] = record.codelistName;

                            fields.Rows.Add(field);
                        }
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
                    // Find if the combindation of the pageobjectname and pagename already exist in the forms collection
                    var foundForm = forms.Where(f => (f.EDCFormIdentifier == record.pageObjectName) && (f.EDCFormName == record.pageName)).SingleOrDefault();
                    if (foundForm == null)
                    {
                        ProtocolEDCForm form = new ProtocolEDCForm
                        {
                            ProtocolMappingId = protocolMappingId,
                            EDCFormIdentifier = record.pageObjectName,
                            EDCFormName = record.pageName,
                            EDCFormTable = record.tableName,
                            CreateDate = DateTime.Now,
                            UpdatedDate = DateTime.Now
                        };

                        forms.Add(form);
                    }
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
