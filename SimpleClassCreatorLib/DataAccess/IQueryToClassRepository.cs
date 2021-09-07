using System.Data;
using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.DataAccess
{
    public interface IQueryToClassRepository
    {
        void ChangeConnectionString(string connectionString);

        string GetPrimaryKeyColumn(TableQuery tableQuery);
        
        DataTable GetSchema(string query);
    }
}