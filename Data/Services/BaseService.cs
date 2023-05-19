using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Services
{
    public abstract class BaseService
    {
        protected WrDbContext context;
        public BaseService(IDbContextFactory<WrDbContext> dbFactory)
        {
            context = dbFactory.CreateDbContext();
        }
    }
}
