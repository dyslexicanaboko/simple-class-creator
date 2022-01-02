using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.DataAccess
{
    public interface IQueryToClassRepository
        : IBaseRepository
    {
        SchemaQuery GetSchema(TableQuery tableQuery, string query);
    }
}