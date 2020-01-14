using SimpleClassCreator;
using SimpleClassCreator.Code_Factory;
using SimpleClassCreator.DTO;
using System.Text;

namespace SimpleClassCreatorUI
{
    internal static class Proxy
    {
        public static ConnectionResult TestConnectionString(string connectionString)
        {
            return Client().TestConnectionString(connectionString);
        }

        public static StringBuilder BuildClass(ClassParameters parameters)
        {
            return Client().BuildClass(parameters);
        }

        public static StringBuilder BuildGridViewColumns(ClassParameters parameters)
        {
            return Client().BuildGridViewColumns(parameters);
        }

        public static AssemblyInfo GetClassProperties(string assembly, string className)
        {
            return Client().GetClassProperties(assembly, className);
        }

        public static string GenerateDto(string assembly, string className, ClassParameters parameters)
        {
            return Client().GenerateDto(assembly, className, parameters);
        }

        private static GeneratorService Client()
        {
            return new GeneratorService();
        }
    }
}
