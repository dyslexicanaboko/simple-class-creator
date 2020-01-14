using SimpleClassCreator.Code_Factory;
using SimpleClassCreator.DTO;
using System.Text;

namespace SimpleClassCreator
{
    public interface IGeneratorService
    {
        ConnectionResult TestConnectionString(string connectionString);

        StringBuilder BuildClass(ClassParameters parameters);

        AssemblyInfo GetClassProperties(string assembly, string className);

        string GenerateDto(string assembly, string className, ClassParameters parameters);
    }
}
