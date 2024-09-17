using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ErrorLogService : BaseService, IErrorLogService
    {

        private readonly ILogger<ErrorLogService> logger; // Add logger field
        public ErrorLogService(ILogger<ErrorLogService> logger, IDatabaseConnectionService databaseConnectionService, IHttpContextAccessor httpContextAccessor) : base(databaseConnectionService)
        {
            this.logger = logger; // Initialize logger
        }

        public async Task SaveErrorLogAsync(int userId, string Url, Exception InnerException, string Source, string Message, string StackTrace)
        {
            try
            {
                DateTime curDateTime = DateTime.UtcNow;
                ErrorLog newError = new ErrorLog();
                newError.UserId = userId;
                newError.CreateDate = curDateTime;
                newError.Url = Url;
                newError.Source = (Source.Length > 4000) ? Source.Substring(0, 4000) : Source;
                newError.Message = (Message.Length > 4000) ? Message.Substring(0, 4000) : Message;
                if (InnerException != null)
                    newError.InnerException = (InnerException.ToString().Length > 4000) ? InnerException.ToString().Substring(0, 4000) : InnerException.ToString();
                newError.StackTrace = (StackTrace.Length > 4000) ? StackTrace.Substring(0, 4000) : StackTrace;

                context.ErrorLog.Add(newError);
                await context.SaveChangesAsync();
            }
            catch { }
        }
    }
}
