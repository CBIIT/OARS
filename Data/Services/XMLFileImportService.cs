using Microsoft.IdentityModel.Protocols.WsTrust;
using System.Data;
using System.Xml;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
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

        public async void ParseXMLFile(Stream inputFileStream, int protocolMappingId)
        {
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
                if (newField != null && newField["EDC_Field_Identifier"] != null && newField["EDC_Field_Name"] != null)
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
                        if (newDictionary != null && newDictionary["EDC_Item_Name"] != null && newDictionary["EDC_Item_Id"] != null && newDictionary["EDC_Dictionary_Name"] != null)
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
            newField["Create_Date"] = DateTime.Now;
            newField["Update_Date"] = DateTime.Now;

            newField["EDC_Field_Identifier"] = field.Attributes["OID"].Value;
            if(field.Attributes["OID"].Value != null)
            {
                string formIdentifier = field.Attributes["OID"].Value.Split('.')[0];
                formIds.TryGetValue(formIdentifier, out int formId);
                newField["Protocol_EDC_Form_Id"] = formId;
            }
            if (field["CodeListRef"] != null)
            {
                newField["EDC_Dictionary_Name"] = field["CodeListRef"].Attributes["CodeListOID"].Value;
            }

            if (field["Question"] != null)
            {
                XmlNodeList translations = field["Question"].GetElementsByTagName("TranslatedText");
                foreach (XmlNode translation in translations)
                {
                    if (translation.Attributes["xml:lang"].Value == "en")
                    {
                        newField["EDC_Field_Name"] = translation.InnerText;
                        break;
                    }
                }
            }
            return newField;
        }

        private DataRow CreateDictionary(XmlNode dictionaryItem, string dictionaryName, int protocolMappingId, DataTable dictionaries)
        {
            DataRow dictionary = dictionaries.NewRow();
            dictionary["Protocol_Mapping_Id"] = protocolMappingId;
            dictionary["Updated_Date"] = DateTime.Now;
            dictionary["Create_Date"] = DateTime.Now;
            dictionary["EDC_Dictionary_Name"] = dictionaryName;
            dictionary["EDC_Item_Id"] = dictionaryItem.Attributes["CodedValue"].Value;
            XmlNodeList translations = dictionaryItem["Decode"].GetElementsByTagName("TranslatedText");
            foreach (XmlNode translation in translations)
            {
                if (translation.Attributes["xml:lang"].Value == "en")
                {
                    dictionary["EDC_Item_Name"] = translation.InnerText;
                    break;
                }
            }
            return dictionary;
        }
    }
}
