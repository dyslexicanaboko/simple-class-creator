using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;

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
    }
}
