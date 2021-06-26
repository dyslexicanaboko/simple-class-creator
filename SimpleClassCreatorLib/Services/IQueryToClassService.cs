using SimpleClassCreator.Models;
using System.Text;

namespace SimpleClassCreator.Services
{
    public interface IQueryToClassService
    {
        TableQuery ParseTableName(string tableNameQuery);

        StringBuilder GenerateClass(ClassParameters parameters);
     
        StringBuilder GenerateGridViewColumns(ClassParameters parameters);
    }
}