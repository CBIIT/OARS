using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        public ThorDBContext context { get; private set; }
        private OracleConnection? _oracleConnection = null;
        public OracleConnection oracleConnection
        {
            get
            {
                if (_oracleConnection == null)
                {
                    _oracleConnection = (OracleConnection)context.Database.GetDbConnection();
                    if (_oracleConnection.State != ConnectionState.Open)
                    {
                        _oracleConnection.Open();
                    }
                }
                return _oracleConnection;
            }
        }


        public DatabaseConnectionService(IDbContextFactory<ThorDBContext> dbFactory)
        {
            context = dbFactory.CreateDbContext();
        }

        public IDbContextTransaction GetDbTransaction()
        {
            return context.Database.BeginTransaction();
        }
    }
}
