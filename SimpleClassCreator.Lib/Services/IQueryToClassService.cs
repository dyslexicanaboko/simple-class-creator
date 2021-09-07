using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services
{
    public interface IQueryToClassService
    {
        string GenerateClass(ClassParameters parameters);
     
        string GenerateGridViewColumns(ClassParameters parameters);
    }
}