using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class UploadService : BaseService, IUploadService
    {
        private readonly IErrorLogService _errorLogService;

        public UploadService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService) : base(dbFactory)
        {
            _errorLogService = errorLogService;
        }
        public async Task<List<string>> GetStudiesToUploadAsync()
        {
            var studies = new List<string>();

            studies.Add("10323");

            return studies;
        }
    }
}
