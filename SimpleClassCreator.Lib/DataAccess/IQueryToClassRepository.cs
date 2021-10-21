using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.DataAccess
{
    public interface IQueryToClassRepository
    {
        void ChangeConnectionString(string connectionString);

        SchemaQuery GetSchema(TableQuery tableQuery, string query);
    }
}