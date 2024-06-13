namespace TheradexPortal.Data.Services.Abstract
{
    public interface IALSFileImportService
    {
        public Task<List<string>> ParseALSFile(Stream inputFileStream, int protocolMappingId);
    }
}
