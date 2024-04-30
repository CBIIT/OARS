namespace TheradexPortal.Data.Services.Abstract
{
    public interface IXMLFileImportService
    {
        public Task ParseXMLFile(Stream inputFileStream, int protocolMappingId);
    }
}
