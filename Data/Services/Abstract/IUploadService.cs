namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUploadService
    {
        public Task<List<string>> GetStudiesToUploadAsync();
    }
}
