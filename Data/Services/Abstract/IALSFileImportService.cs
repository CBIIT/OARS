namespace TheradexPortal.Data.Services.Abstract
{
    public interface IALSFileImportService
    {
        public void ParseALSFile(Stream inputFileStream, int protocolMappingId);
    }
}
