using Microsoft.EntityFrameworkCore.Storage;
using Oracle.ManagedDataAccess.Client;

namespace OARS.Data.Services.Abstract
{
    public interface IDatabaseConnectionService
    {
        ThorDBContext context { get; }
        OracleConnection oracleConnection { get; }
        IDbContextTransaction GetDbTransaction();
    }
}
