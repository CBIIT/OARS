using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace TheradexPortal.Data.Services
{
    public abstract class BaseService
    {
        protected ThorDBContext context;
        private OracleConnection _oracleConnection = null;
        protected OracleConnection oracleConnection
        {
            get
            {
                if (_oracleConnection == null)
                {
                    _oracleConnection = (OracleConnection)context.Database.GetDbConnection();
                    _oracleConnection.Open();
                    if (context.Database.CurrentTransaction != null)
                    {
                        _oracleConnection.EnlistTransaction((System.Transactions.Transaction)context.Database.CurrentTransaction);
                    }
                }
                return _oracleConnection;
            }
            set
            {
                _oracleConnection = value;
            }
        }   

        public BaseService(IDbContextFactory<ThorDBContext> dbFactory)
        {
            context = dbFactory.CreateDbContext();
        }

    }
}
