using System.Data;
using System.Drawing.Printing;
using System.Xml;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

public class ALSFileImportService : IALSFileImportService
{
    List<string> worksheetNames = new List<string> { "Forms", "Fields", "DataDictionaryEntries" };
    private readonly IProtocolEDCFormService _formService;
    private readonly IProtocolEDCFieldService _fieldService;
    private readonly IProtocolEDCDictionaryService _dictionaryService;

    public ALSFileImportService(IProtocolEDCFormService formService, IProtocolEDCFieldService fieldService, IProtocolEDCDictionaryService dictionaryService)
    { 
        _formService = formService;
        _fieldService = fieldService;
        _dictionaryService = dictionaryService;
	}

    public async void ParseALSFile(Stream inputFileStream, int protocolMappingId)
    {
        if (inputFileStream.Position > 0)
        {
            inputFileStream.Position = 0;
        }
        XmlDocument document = new XmlDocument();
        document.Load(inputFileStream);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
        nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
        nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
        nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

        List<ProtocolEDCForm> forms = new List<ProtocolEDCForm>();
        Dictionary<string, int> formIds = new Dictionary<string, int>();
        List<ProtocolEDCField> fields = new List<ProtocolEDCField>();
        DataTable dictionaries = SetUpDictionaryTable();

        foreach (XmlNode node in document.DocumentElement.SelectNodes("//ss:Worksheet", nsmgr)){
            string currWorksheet = node.Attributes["ss:Name"].Value;
            if (worksheetNames.Contains(currWorksheet))
            {
                XmlNodeList? rows = node.SelectNodes("ss:Table/ss:Row", nsmgr);
                if (rows != null && rows.Count > 0)
                {
                    List<String> columns = new List<String>();
                    foreach (XmlNode cell in rows[0].SelectNodes("ss:Cell/ss:Data", nsmgr))
                    {
                        columns.Add(cell.InnerText);
                    }
                    for (int i = 1; i < rows.Count; i++)
                    {
                        XmlNodeList? cells = rows[i].SelectNodes("ss:Cell", nsmgr);

                        // based on current worksheet, parse the data
                        if(cells != null && cells.Count > 0)
                        {
                            if (currWorksheet == "Forms")
                            {
                                ProtocolEDCForm form = CreateForm(columns, cells, nsmgr, protocolMappingId);
                                if (form != null && form.EDCFormName != null && form.EDCFormIdentifier != null)
                                {
                                    forms.Add(form);
                                } 
                            }
                            else if (currWorksheet == "Fields")
                            {                     
                                ProtocolEDCField field = CreateField(columns, cells, nsmgr, formIds);
                                if(field != null && field.EDCFieldIdentifier != null && field.EDCFieldName != null)
                                {
                                    fields.Add(field);
                                }
                            }
                            else if (currWorksheet == "DataDictionaryEntries")
                            {
                                DataRow entry = CreateDictionary(columns, cells, nsmgr, protocolMappingId, dictionaries);
                                if(entry != null && entry["EDC_Item_Name"] != null && entry["EDC_Item_Id"] != null && entry["EDC_Dictionary_Name"] != null) {
                                    dictionaries.Rows.Add(entry);
                                }
                            }
                        }
                    }
                }
                if (forms.Count > 0 || fields.Count > 0 || dictionaries.Rows.Count > 0)
                {
                    if (currWorksheet == "Forms")
                    {
                        await _formService.BulkSaveForms(forms);
                        foreach(ProtocolEDCForm form in forms)
                        {
                           formIds.Add(form.EDCFormIdentifier, form.ProtocolEDCFormId); 
                        }
                    }
                    else if (currWorksheet == "Fields")
                    {
                        await _fieldService.BulkSaveFields(fields);
                    }
                    else if (currWorksheet == "DataDictionaryEntries")
                    {
                        await _dictionaryService.BulkSaveDictionaries(dictionaries);
                    }
                }
            }
        }
    }

    private DataTable SetUpDictionaryTable()
    {
        DataTable dictionaries = new DataTable();
        dictionaries.Columns.Add("Protocol_EDC_Dictionary_Id", typeof(int));
        dictionaries.Columns.Add("Protocol_Mapping_Id", typeof(int));
        dictionaries.Columns.Add("EDC_Dictionary_Name", typeof(string));
        dictionaries.Columns.Add("EDC_Item_Id", typeof(string));
        dictionaries.Columns.Add("EDC_Item_Name", typeof(string));
        dictionaries.Columns.Add("Create_Date", typeof(DateTime));
        dictionaries.Columns.Add("Updated_Date", typeof(DateTime));
        return dictionaries;
    }

    private ProtocolEDCForm CreateForm(List<String> columns, XmlNodeList cells, XmlNamespaceManager nsmgr, int protocolMappingId)
    {
        ProtocolEDCForm form = new ProtocolEDCForm();
        form.ProtocolMappingId = protocolMappingId;
        form.UpdatedDate = DateTime.Now;
        form.CreateDate = DateTime.Now;
        int columnIndex = 0;
        for (int i = 0; i < cells.Count; i++)
        {
            XmlNode cell = cells[i];
            if (cell.Attributes["ss:Index"] != null)
            {
                int indexJump = int.Parse(cell.Attributes["ss:Index"].Value) - 1;
                if (indexJump > columns.Count())
                {
                    break;
                }
                columnIndex = indexJump;
            }
            XmlNode dataNode = cell.SelectSingleNode("ss:Data", nsmgr);
            if (dataNode != null)
            {
                if (columns[columnIndex] == "OID")
                {
                    form.EDCFormIdentifier = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "DraftFormName")
                {
                    form.EDCFormName = dataNode.InnerText;
                }
            }
            columnIndex++;
        }
        return form;
    }

    private ProtocolEDCField CreateField(List<String> columns, XmlNodeList cells, XmlNamespaceManager nsmgr, Dictionary<string, int> formIds)
    {
        ProtocolEDCField field = new ProtocolEDCField();
        field.UpdateDate = DateTime.Now;
        field.CreateDate = DateTime.Now;
        int columnIndex = 0;

        for (int i = 0; i < cells.Count; i++)
        {
            XmlNode cell = cells[i];
            if (cell.Attributes["ss:Index"] != null)
            {
                int indexJump = int.Parse(cell.Attributes["ss:Index"].Value) - 1;
                if (indexJump > columns.Count())
                {
                    break;
                }
                columnIndex = indexJump;
            }
            XmlNode dataNode = cell.SelectSingleNode("ss:Data", nsmgr);
            if (dataNode != null)
            {
                if (columns[columnIndex] == "FormOID")
                {
                    // need to get the integer ID of the form in the DB based on the name
                    if (formIds.ContainsKey(dataNode.InnerText))
                    {
                        field.ProtocolEDCFormId = formIds[dataNode.InnerText];
                    }
                }
                else if (columns[columnIndex] == "FieldOID")
                {
                    field.EDCFieldIdentifier = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "PreText")
                {
                    field.EDCFieldName = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "DataDictionaryName")
                {
                    field.EDCDictionaryName = dataNode.InnerText;
                }
            }
            columnIndex++;
        }
        return field;
    }

    private DataRow CreateDictionary(List<String> columns, XmlNodeList cells, XmlNamespaceManager nsmgr, int protocolMappingId, DataTable dictionaries)
    {
        DataRow dictionary = dictionaries.NewRow();
        dictionary["Protocol_Mapping_Id"] = protocolMappingId;
        dictionary["Updated_Date"] = DateTime.Now;
        dictionary["Create_Date"] = DateTime.Now;
        int columnIndex = 0;

        for (int i = 0; i < cells.Count; i++)
        {
            XmlNode cell = cells[i];
            if (cell.Attributes["ss:Index"] != null)
            {
                int indexJump = int.Parse(cell.Attributes["ss:Index"].Value) - 1;
                if (indexJump > columns.Count())
                {
                    break;
                }
                columnIndex = indexJump;
            }
            XmlNode dataNode = cell.SelectSingleNode("ss:Data", nsmgr);
            if (dataNode != null)
            {
                if (columns[columnIndex] == "DataDictionaryName")
                {
                    // double check how we want to handle this, the DB has an int but we only have the string name
                    dictionary["EDC_Dictionary_Name"] = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "CodedData")
                {
                    dictionary["EDC_Item_Id"] = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "UserDataString")
                {
                    dictionary["EDC_Item_Name"] = dataNode.InnerText;
                }
            }
            columnIndex++;
        }
        return dictionary;
    }
}
