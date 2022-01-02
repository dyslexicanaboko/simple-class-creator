using SimpleClassCreator.Lib.Models;
using System.Data;

namespace SimpleClassCreator.Lib.DataAccess
{
    public class GeneralDatabaseQueries
        : BaseRepository, IGeneralDatabaseQueries
    {
        public ConnectionResult TestConnectionString(string connectionString)
        {
            ChangeConnectionString(connectionString);

            var result = TestConnectionString();

            return result;
        }

        public DataTable GetRowData(string sql) => ExecuteDataTable(sql);
    }
}
