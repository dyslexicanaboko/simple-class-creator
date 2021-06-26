using SimpleClassCreator.Models;

namespace SimpleClassCreator.DataAccess
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
    }
}
