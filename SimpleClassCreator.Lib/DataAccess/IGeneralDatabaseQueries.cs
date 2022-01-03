using SimpleClassCreator.Lib.Models;
using System.Data;

namespace SimpleClassCreator.Lib.DataAccess
{
    public interface IGeneralDatabaseQueries
        : IBaseRepository
    {
        ConnectionResult TestConnectionString(string connectionString);

        DataTable GetRowData(string sql);
    }
}