using SimpleClassCreator.Models;
using System.Data;

namespace SimpleClassCreator
{
    public interface IQueryToClassRepository
    {
        void ChangeConnectionString(string connectionString);

        string GetPrimaryKeyColumn(TableQuery tableQuery);
        
        DataTable GetSchema(string query);
    }
}