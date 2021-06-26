using SimpleClassCreator.Models;

namespace SimpleClassCreator.DataAccess
{
    public interface IGeneralDatabaseQueries
    {
        ConnectionResult TestConnectionString(string connectionString);
    }
}