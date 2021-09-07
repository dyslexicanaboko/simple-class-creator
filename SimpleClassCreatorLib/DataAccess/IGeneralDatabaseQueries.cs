using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.DataAccess
{
    public interface IGeneralDatabaseQueries
    {
        ConnectionResult TestConnectionString(string connectionString);
    }
}