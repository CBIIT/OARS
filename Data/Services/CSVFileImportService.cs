using MimeKit;
using System.Data;
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

    public async Task ParseCSVField(MemoryStream inputFileStream, int protocolMappingId)
    {
        var forms = await _formService.GetProtocolEDCFormsByProtocolMappingId(protocolMappingId);

        if (forms.Count > 0)
        {
            DataTable fields = SetUpFieldTable();
            using var reader = new StreamReader(inputFileStream, Encoding.UTF8);

            string content = Encoding.UTF8.GetString(inputFileStream.ToArray());
            string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // Process each line (skip the header row)
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                string formID = values[1];
                string fieldID = values[12];
                string dictionaryName = values[6];
                string fieldLabel = values[5];
                string order = values[2];

                var EDCForm = forms.Where(f => f.EDCFormIdentifier == formID).FirstOrDefault();
                if ((!string.IsNullOrEmpty(formID) || !string.IsNullOrEmpty(fieldID)) && EDCForm != null)
                {
                    DataRow field = fields.NewRow();
                    field["Update_Date"] = DateTime.Now;
                    field["Create_Date"] = DateTime.Now;
                    field["Protocol_EDC_Form_Id"] = EDCForm.ProtocolEDCFormId;
                    field["EDC_Field_Identifier"] = fieldID;
                    field["EDC_Field_Name"] = fieldLabel;
                    field["EDC_Dictionary_Name"] = dictionaryName;

                    fields.Rows.Add(field);
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
        }
    }

    public async Task ParseCSVFileForm(MemoryStream inputFileStream, int protocolMappingId)
    {
        List<ProtocolEDCForm> forms = new List<ProtocolEDCForm>();

        using var reader = new StreamReader(inputFileStream, Encoding.UTF8);

        string content = Encoding.UTF8.GetString(inputFileStream.ToArray());
        string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        // Process each line (skip the header row)
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            ProtocolEDCForm form = new ProtocolEDCForm
            {
                ProtocolMappingId = protocolMappingId,
                EDCFormIdentifier = values[13],
                EDCFormName = values[10],
                CreateDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            forms.Add(form);
        }

        bool saveFormResult = await _formService.BulkSaveForms(forms);
        if (!saveFormResult)
        {
            throw new Exception("Error uploading file, clear uploads and try again.");
        }
    }

    public async Task ParseCSVFileMeta(MemoryStream inputFileStream, int protocolMappingId)
    {
        DataTable dictionaries = SetUpDictionaryTable();
        using var reader = new StreamReader(inputFileStream, Encoding.UTF8);

        string content = Encoding.UTF8.GetString(inputFileStream.ToArray());
        string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        // Process each line (skip the header row)
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            string DictionaryName = values[0];
            string DictionaryShortValue = values[2];
            string DictionaryLongValue = values[3];

            if (!string.IsNullOrEmpty(DictionaryName))
            {
                DataRow dictionary = dictionaries.NewRow();
                dictionary["Updated_Date"] = DateTime.Now;
                dictionary["Create_Date"] = DateTime.Now;
                dictionary["Protocol_Mapping_Id"] = protocolMappingId;
                dictionary["EDC_Item_Id"] = DictionaryShortValue;
                dictionary["EDC_Item_Name"] = DictionaryLongValue;
                dictionary["EDC_Dictionary_Name"] = DictionaryName;

                dictionaries.Rows.Add(dictionary);
            }
        }

        bool saveDictResult = await _dictionaryService.BulkSaveDictionaries(dictionaries);
        if (!saveDictResult)
        {
            throw new Exception("Error uploading dictionary file, clear uploads and try again.");
        }
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
