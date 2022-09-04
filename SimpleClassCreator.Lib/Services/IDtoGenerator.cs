using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Models.Meta;

namespace SimpleClassCreator.Lib.Services
{
    public interface IDtoGenerator
    {
        void LoadAssembly(string assemblyPath);

        MetaAssembly GetClassProperties(string className);

        string MakeDto(string className, DtoMakerParameters parameters);

        MetaAssembly GetListOfClasses();
    }
}