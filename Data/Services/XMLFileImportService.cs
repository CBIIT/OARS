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
            List<ProtocolEDCForm> formsToSave = new List<ProtocolEDCForm>();
            foreach (XmlNode form in forms)
            {
                ProtocolEDCForm newForm = CreateForm(form, protocolMappingId);
                formsToSave.Add(newForm);
            }
            if(formsToSave.Count > 0)
            {
                await _formService.BulkSaveForms(formsToSave);
            }
            
            XmlNodeList fields = document.GetElementsByTagName("ItemDef");
            List<ProtocolEDCField> fieldsToSave = new List<ProtocolEDCField>();
            foreach (XmlNode field in fields)
            {
                ProtocolEDCField newField = CreateField(field);
                fieldsToSave.Add(newField);
            }
            if(fieldsToSave.Count > 0)
            {
                await _fieldService.BulkSaveFields(fieldsToSave);
            }

            XmlNodeList dictionaries = document.GetElementsByTagName("CodeList");
            List<ProtocolEDCDictionary> dictionariesToSave = new List<ProtocolEDCDictionary>();
            foreach (XmlNode dictionary in dictionaries)
            {
                string dictionaryName = dictionary.Attributes["OID"].Value;
                XmlNodeList items = dictionary.SelectNodes("*[local-name()='CodeListItem']");
                foreach (XmlNode item in items)
                {
                    ProtocolEDCDictionary newDictionary = CreateDictionary(item, dictionaryName, protocolMappingId);
                    dictionariesToSave.Add(newDictionary);
                }
            }
            if(dictionariesToSave.Count > 0)
            {
                await _dictionaryService.BulkSaveDictionaries(dictionariesToSave);
            }
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

        private ProtocolEDCField CreateField(XmlNode field)
        {
            ProtocolEDCField newField = new ProtocolEDCField();
            newField.CreateDate = DateTime.Now;
            newField.EDCFieldIdentifier = field.Attributes["OID"].Value;
            if (field["CodeListRef"] != null)
            {
                newField.EDCDictionaryName = field["CodeListRef"].Attributes["CodeListOID"].Value;
            }

            if (field["Question"] != null)
            {
                XmlNodeList translations = field["Question"].GetElementsByTagName("TranslatedText");
                foreach (XmlNode translation in translations)
                {
                    if (translation.Attributes["xml:lang"].Value == "en")
                    {
                        newField.EDCFieldName = translation.InnerText;
                        break;
                    }
                }
            }
            return newField;
        }

        private ProtocolEDCDictionary CreateDictionary(XmlNode dictionaryItem, string dictionaryName, int protocolMappingId)
        {
            ProtocolEDCDictionary newDictionary = new ProtocolEDCDictionary();
            newDictionary.CreateDate = DateTime.Now;
            newDictionary.UpdatedDate = DateTime.Now;
            newDictionary.EDCDictionaryName = dictionaryName;
            newDictionary.EDCItemId = dictionaryItem.Attributes["CodedValue"].Value;
            XmlNodeList translations = dictionaryItem["Decode"].GetElementsByTagName("TranslatedText");
            foreach (XmlNode translation in translations)
            {
                if (translation.Attributes["xml:lang"].Value == "en")
                {
                    newDictionary.EDCItemName = translation.InnerText;
                    break;
                }
            }
            return newDictionary;
        }
    }
}
