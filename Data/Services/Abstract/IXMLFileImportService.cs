namespace TheradexPortal.Data.Services.Abstract
{
    public interface IXMLFileImportService
    {
        public void ParseXMLFile(Stream inputFileStream, int protocolMappingId);
    }
}
