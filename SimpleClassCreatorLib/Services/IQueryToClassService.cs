using SimpleClassCreator.Models;

namespace SimpleClassCreator.Services
{
    public interface IQueryToClassService
    {
        string GenerateClass(ClassParameters parameters);
     
        string GenerateGridViewColumns(ClassParameters parameters);
    }
}