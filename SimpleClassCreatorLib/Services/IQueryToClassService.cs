using SimpleClassCreator.Models;
using System.Text;

namespace SimpleClassCreator.Services
{
    public interface IQueryToClassService
    {
        StringBuilder GenerateClass(ClassParameters parameters);
     
        StringBuilder GenerateGridViewColumns(ClassParameters parameters);
    }
}