using SimpleClassCreator.Code_Factory;
using SimpleClassCreator.DTO;
using System.Text;

namespace SimpleClassCreator
{
    public class GeneratorService 
        : IGeneratorService
    {
        public ConnectionResult TestConnectionString(string connectionString)
        {
            return DataAccess.DAL.TestConnectionString(connectionString);
        }

        public StringBuilder BuildClass(ClassParameters parameters)
        {
            return Generator.Execute(parameters);
        }

        public StringBuilder BuildGridViewColumns(ClassParameters parameters)
        {
            return Generator.GenerateGridViewColumns(parameters);
        }

        public AssemblyInfo GetClassProperties(string assembly, string className)
        {
            return new DtoGenerator(assembly).GetClassProperties(className);
        }

        public string GenerateDto(string assembly, string className, ClassParameters parameters)
        {
            return new DtoGenerator(assembly).MakeDto(className, parameters);
        }
    }
}
