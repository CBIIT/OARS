namespace TheradexPortal.Data.Services.Abstract
{
    public interface ICSVFileImportService
    {
        public Task ParseCSVField(MemoryStream inputFileStream, int protocolMappingId);
        public Task ParseCSVFileForm(MemoryStream inputFileStream, int protocolMappingId);
        public Task ParseCSVFileMeta(MemoryStream inputFileStream, int protocolMappingId);
    }
}
