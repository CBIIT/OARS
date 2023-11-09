using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IErrorLogService
    {
        public Task SaveErrorLogAsync(int userId, string Url, Exception InnerException, string Source, string Message, string StackTrace);
    }
}
