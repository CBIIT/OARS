using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace TheradexPortal.Data.Services
{
    public abstract class BaseService
    {
        protected ThorDBContext context;
        protected OracleConnection oracleConnection;
        public BaseService(IDbContextFactory<ThorDBContext> dbFactory)
        {
            context = dbFactory.CreateDbContext();
            var dbConnection = context.Database.GetDbConnection();
            oracleConnection = (OracleConnection)dbConnection;
        }
    }
}
