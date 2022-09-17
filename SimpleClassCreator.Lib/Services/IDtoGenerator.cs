using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleClassCreator.Lib.Services
{
    public interface IDtoGenerator
    {
        bool IsLoaded { get; }

        Assembly AssemblyReference { get; }

        void LoadAssembly(string assemblyPath);

        void LoadAssembly(CompilerResult dynamicAssembly);

        Type GetClass(string fullyQualifiedClassName);

        IList<ClassMemberStrings> GetProperties(Type metaClass);

        MetaAssembly GetMetaClassProperties(string className);

        MetaAssembly GetMetaClasses();
    }
}