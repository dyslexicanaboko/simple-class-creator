using SimpleClassCreator.Lib.Models;
using System;

namespace SimpleClassCreator.Lib.Services
{
    public interface IDtoGenerator
    {
        void LoadAssembly(string assemblyPath);

        AssemblyInfo GetClassProperties(string className);

        string GetTypeAsString(Type target);

        string MakeDto(string className, ClassParameters parameters);

        Type PrintClass(string className);

        void PrintClasses();
    }
}