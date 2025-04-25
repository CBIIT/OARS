using Microsoft.IdentityModel.Protocols.WsTrust;
using System.Data;
using System.Xml;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class XMLFileImportService : IXMLFileImportService
    {
        private readonly IProtocolEDCFormService _formService;
        private readonly IProtocolEDCFieldService _fieldService;
        private readonly IProtocolEDCDictionaryService _dictionaryService;

        public XMLFileImportService(IProtocolEDCFormService formService, IProtocolEDCFieldService fieldService, IProtocolEDCDictionaryService dictionaryService)
        {
            _formService = formService;
            _fieldService = fieldService;
            _dictionaryService = dictionaryService;
        }

        public async Task<List<string>> ParseXMLFile(Stream inputFileStream, int protocolMappingId)
        {
            var processInfo = new List<string>();

            if (inputFileStream.Position > 0)
            {
                inputFileStream.Position = 0;
            }
            XmlDocument document = new XmlDocument();
            document.Load(inputFileStream);

            XmlNodeList forms = document.GetElementsByTagName("FormDef");
            Dictionary<string, int> formIds = new Dictionary<string, int>();
            List<ProtocolEDCForm> formsToSave = new List<ProtocolEDCForm>();
            foreach (XmlNode form in forms)
            {
                ProtocolEDCForm newForm = CreateForm(form, protocolMappingId);
                if (newForm != null && newForm.EDCFormName != null && newForm.EDCFormIdentifier != null)
                {
                    formsToSave.Add(newForm);
                }
            }
            if(formsToSave.Count > 0)
            {
                bool saveFormResult = await _formService.BulkSaveForms(formsToSave);
                if (!saveFormResult)
                {
                    throw new Exception("Error uploading file, clear uploads and try again.");
                }
                foreach (ProtocolEDCForm form in formsToSave)
                {
                    formIds.Add(form.EDCFormIdentifier, form.ProtocolEDCFormId);
                }
            }
            
            XmlNodeList fields = document.GetElementsByTagName("ItemDef");
            DataTable fieldsToSave = SetUpFieldTable();
            foreach (XmlNode field in fields)
            {
                DataRow newField = CreateField(field, formIds, fieldsToSave);
                if (newField != null && newField["EDC_FIELD_IDENTIFIER"] != null && newField["EDC_FIELD_NAME"] != null)
                {
                    fieldsToSave.Rows.Add(newField);
                }
            }
            if(fieldsToSave.Rows.Count > 0)
            {
               bool saveFieldResult = await _fieldService.BulkSaveFields(fieldsToSave);
                if (!saveFieldResult)
                {
                    throw new Exception("Error uploading file, clear uploads and try again.");
                }
            }

            XmlNodeList dictionaries = document.GetElementsByTagName("CodeList");
            DataTable dictionariesToSave = SetUpDictionaryTable();
            foreach (XmlNode dictionary in dictionaries)
            {
                string dictionaryName = dictionary.Attributes["OID"].Value;
                if (dictionaryName != null)
                {
                    XmlNodeList items = dictionary.SelectNodes("*[local-name()='CodeListItem']");
                    foreach (XmlNode item in items)
                    {
                        DataRow newDictionary = CreateDictionary(item, dictionaryName, protocolMappingId, dictionariesToSave);
                        if (newDictionary != null && newDictionary["EDC_ITEM_NAME"] != null && newDictionary["EDC_ITEM_ID"] != null && newDictionary["EDC_DICTIONARY_NAME"] != null)
                        {
                            dictionariesToSave.Rows.Add(newDictionary);
                        }

                    }
                }
            }
            if(dictionariesToSave.Rows.Count > 0)
            {
                bool saveDictResult = await _dictionaryService.BulkSaveDictionaries(dictionariesToSave);
                if (!saveDictResult)
                {
                    throw new Exception("Error uploading file, clear uploads and try again.");
                }
            }

            processInfo.Add("Form records inserted: " + formsToSave.Count.ToString());
            processInfo.Add("Field records inserted: " + fieldsToSave.Rows.Count.ToString());
            processInfo.Add("Dictionary records inserted: " + dictionariesToSave.Rows.Count.ToString());

            return processInfo;
        }

        private DataTable SetUpDictionaryTable()
        {
            DataTable dictionaries = new DataTable();
            dictionaries.Columns.Add("PROTOCOL_EDC_DICTIONARY_ID", typeof(int));
            dictionaries.Columns.Add("PROTOCOL_MAPPING_ID", typeof(int));
            dictionaries.Columns.Add("EDC_ITEM_ID", typeof(string));
            dictionaries.Columns.Add("EDC_ITEM_NAME", typeof(string));
            dictionaries.Columns.Add("EDC_DICTIONARY_NAME", typeof(string));
            dictionaries.Columns.Add("CREATE_DATE", typeof(DateTime));
            dictionaries.Columns.Add("UPDATED_DATE", typeof(DateTime));
            return dictionaries;
        }

        private DataTable SetUpFieldTable()
        {
            DataTable fields = new DataTable();
            fields.Columns.Add("PROTOCOL_EDC_FIELD_ID", typeof(int));
            fields.Columns.Add("PROTOCOL_EDC_FORM_ID", typeof(int));
            fields.Columns.Add("EDC_FIELD_IDENTIFIER", typeof(string));
            fields.Columns.Add("EDC_FIELD_NAME", typeof(string));
            fields.Columns.Add("EDC_DICTIONARY_NAME", typeof(string));
            fields.Columns.Add("CREATE_DATE", typeof(DateTime));
            fields.Columns.Add("UPDATE_DATE", typeof(DateTime));
            return fields;
        }

        private ProtocolEDCForm CreateForm(XmlNode form, int protocolMappingId)
        {
            ProtocolEDCForm newForm = new ProtocolEDCForm();
            newForm.CreateDate = DateTime.Now;
            newForm.UpdatedDate = DateTime.Now;
            newForm.ProtocolMappingId = protocolMappingId;
            newForm.EDCFormIdentifier = form.Attributes["OID"].Value;
            newForm.EDCFormName = form.Attributes["Name"].Value;

            return newForm;
        }

        private DataRow CreateField(XmlNode field, Dictionary<string, int> formIds, DataTable fields)
        {
            DataRow newField = fields.NewRow();
            newField["CREATE_DATE"] = DateTime.Now;
            newField["UPDATE_DATE"] = DateTime.Now;

            newField["EDC_FIELD_IDENTIFIER"] = field.Attributes["OID"].Value;
            if(field.Attributes["OID"].Value != null)
            {
                string formIdentifier = field.Attributes["OID"].Value.Split('.')[0];
                formIds.TryGetValue(formIdentifier, out int formId);
                newField["PROTOCOL_EDC_FORM_ID"] = formId;
            }
            if (field["CodeListRef"] != null)
            {
                newField["EDC_DICTIONARY_NAME"] = field["CodeListRef"].Attributes["CodeListOID"].Value;
            }

            if (field["Question"] != null)
            {
                XmlNodeList translations = field["Question"].GetElementsByTagName("TranslatedText");
                foreach (XmlNode translation in translations)
                {
                    if (translation.Attributes["xml:lang"].Value == "en")
                    {
                        newField["EDC_FIELD_NAME"] = translation.InnerText;
                        break;
                    }
                }
            }
            return newField;
        }

        private DataRow CreateDictionary(XmlNode dictionaryItem, string dictionaryName, int protocolMappingId, DataTable dictionaries)
        {
            DataRow dictionary = dictionaries.NewRow();
            dictionary["PROTOCOL_MAPPING_ID"] = protocolMappingId;
            dictionary["UPDATED_DATE"] = DateTime.Now;
            dictionary["CREATE_DATE"] = DateTime.Now;
            dictionary["EDC_DICTIONARY_NAME"] = dictionaryName;
            dictionary["EDC_ITEM_ID"] = dictionaryItem.Attributes["CodedValue"].Value;
            XmlNodeList translations = dictionaryItem["Decode"].GetElementsByTagName("TranslatedText");
            foreach (XmlNode translation in translations)
            {
                if (translation.Attributes["xml:lang"].Value == "en")
                {
                    dictionary["EDC_ITEM_NAME"] = translation.InnerText;
                    break;
                }
            }
            return dictionary;
        }
    }
}
