using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Services
{
    public abstract class BaseService
    {
        protected ThorDBContext context;
        public BaseService(IDbContextFactory<ThorDBContext> dbFactory)
        {
            context = dbFactory.CreateDbContext();
        }
    }
}
