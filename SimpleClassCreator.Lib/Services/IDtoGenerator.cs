using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services
{
    public interface IDtoGenerator
    {
        void LoadAssembly(string assemblyPath);

        AssemblyInfo GetClassProperties(string className);

        string MakeDto(string className, DtoMakerParameters parameters);

        AssemblyInfo GetListOfClasses();
    }
}