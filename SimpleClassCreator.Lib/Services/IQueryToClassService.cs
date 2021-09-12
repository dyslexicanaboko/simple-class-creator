using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services
{
    public interface IQueryToClassService
    {
        string GenerateClass(QueryToClassParameters parameters);
     
        string GenerateGridViewColumns(QueryToClassParameters parameters);
    }
}