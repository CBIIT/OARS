namespace OARS.Data.Services.Abstract
{
    public interface IXMLFileImportService
    {
        public Task<List<string>> ParseXMLFile(Stream inputFileStream, int protocolMappingId);
    }
}
