namespace TheradexPortal.Data.Services.Abstract
{
    public interface ICSVFileImportService
    {
        public Task<List<string>> ParseCSVField(MemoryStream inputFileStream, int protocolMappingId);
        public Task<List<string>> ParseCSVFileForm(MemoryStream inputFileStream, int protocolMappingId);
        public Task<List<string>> ParseCSVFileMeta(MemoryStream inputFileStream, int protocolMappingId);
    }
}
