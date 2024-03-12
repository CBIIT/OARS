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
        List<ProtocolEDCField> fields = new List<ProtocolEDCField>();
        List<ProtocolEDCDictionary> dictionaries = new List<ProtocolEDCDictionary>();

        foreach (XmlNode node in document.DocumentElement.SelectNodes("//ss:Worksheet", nsmgr)){
            string currWorksheet = node.Attributes["ss:Name"].Value;
            Console.WriteLine($"-------WORKSHEET: {currWorksheet}");
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
                    Console.WriteLine(columns.Count);
                    Console.WriteLine(columns[0]);
                    for (int i = 1; i < rows.Count; i++)
                    {
                        XmlNodeList? cells = rows[i].SelectNodes("ss:Cell", nsmgr);

                        // based on current worksheet, parse the data
                        if(cells != null && cells.Count > 0)
                        {
                            if (currWorksheet == "Forms1")
                            {
                                ProtocolEDCForm form = CreateForm(columns, cells, nsmgr, protocolMappingId);
                                forms.Add(form);
                            }
                            else if (currWorksheet == "Fields1")
                            {
                                ProtocolEDCField field = CreateField(columns, cells, nsmgr);
                                fields.Add(field);
                            }
                            else if (currWorksheet == "DataDictionaryEntries")
                            {
                                ProtocolEDCDictionary entry = CreateDictionary(columns, cells, nsmgr, protocolMappingId);
                                dictionaries.Add(entry);
                            }
                        }
                    }
                }
                if (forms.Count > 0 || fields.Count > 0 || dictionaries.Count > 0)
                {
                    if (currWorksheet == "Forms")
                    {
                        await _formService.BulkSaveForms(forms);
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

    private ProtocolEDCField CreateField(List<String> columns, XmlNodeList cells, XmlNamespaceManager nsmgr)
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
                    // double check how we want to handle this, the DB has an int but we only have the string name
                    field.ProtocolEDCFormId = dataNode.InnerText;
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

    private ProtocolEDCDictionary CreateDictionary(List<String> columns, XmlNodeList cells, XmlNamespaceManager nsmgr, int protocolMappingId)
    {
        ProtocolEDCDictionary dictionary = new ProtocolEDCDictionary();
        dictionary.ProtocolMappingId = protocolMappingId;
        dictionary.UpdatedDate = DateTime.Now;
        dictionary.CreateDate = DateTime.Now;
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
                    dictionary.EDCDictionaryName = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "CodedData")
                {
                    dictionary.EDCItemId = dataNode.InnerText;
                }
                else if (columns[columnIndex] == "UserDataString")
                {
                    dictionary.EDCItemName = dataNode.InnerText;
                }
            }
            columnIndex++;
        }
        return dictionary;
    }
}
