namespace TheradexPortal.Data.Services.Abstract
{
    public interface IALSFileImportService
    {
        public Task ParseALSFile(Stream inputFileStream, int protocolMappingId);
    }
}
