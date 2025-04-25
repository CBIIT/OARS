using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public abstract class BaseService
    {
        protected IDatabaseConnectionService _databaseConnectionService;

        protected ThorDBContext context
        {
            get
            {
                return _databaseConnectionService.context;
            }
        }
        protected OracleConnection oracleConnection
        {
            get
            {
                return _databaseConnectionService.oracleConnection;
            }
        }   

        public BaseService(IDatabaseConnectionService databaseConnectionService)
        {
            _databaseConnectionService = databaseConnectionService;
        }

    }
}
