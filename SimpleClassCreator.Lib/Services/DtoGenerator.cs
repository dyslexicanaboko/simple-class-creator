using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleClassCreator.Lib.Services
{
    public class DtoGenerator
        : IDtoGenerator
    {
        private string _assemblyPath;

        private string _fileName;
        
        public Assembly AssemblyReference { get; private set; }

        public bool IsLoaded => AssemblyReference != null;

        public void LoadAssembly(string assemblyPath)
        {
            _assemblyPath = assemblyPath;

            _fileName = Path.GetFileName(assemblyPath);

            AssemblyReference = Assembly.LoadFile(_assemblyPath);
        }

        public void LoadAssembly(CompilerResult dynamicAssembly)
        {
            _assemblyPath = dynamicAssembly.AssemblyPath;

            _fileName = Path.GetFileName(dynamicAssembly.AssemblyPath);

            AssemblyReference = dynamicAssembly.VirtualAssembly;
        }

        private MetaAssembly GetEmptyMetaAssembly()
        {
            var asm = new MetaAssembly {Name = _fileName};

            return asm;
        }

        public MetaAssembly GetMetaClasses()
        {
            var asm = GetEmptyMetaAssembly();

            var lst = AssemblyReference
                .GetTypes()
                .TakeWhile(x => x.IsClass)
                .Select(x => x.FullName);

            asm.Add(lst);

            return asm;
        }

        public IList<ClassMemberStrings> GetProperties(Type metaClass)
        {
            var lst = metaClass.GetProperties().Select(x => new ClassMemberStrings(x)).ToList();

            return lst;
        }

        public MetaAssembly GetMetaClassProperties(string className)
        {
            var t = GetClass(className);

            var asm = GetEmptyMetaAssembly();

            var cInfo = asm.Add(className);

            foreach (var pi in t.GetProperties())
            {
                Console.WriteLine(pi.Name);

                var cms = new ClassMemberStrings(pi);

                var pt = pi.PropertyType;

                cInfo.Properties.Add(new MetaProperty
                {
                    Name = pi.Name,
                    TypeName = cms.SystemTypeAlias,
                    IsPrimitive = pt.IsValueType || pt == typeof(string),
                    IsEnum = pt.IsEnum,
                    IsInterface = pt.IsInterface,
                    IsSerializable = pt.IsDefined(typeof(SerializableAttribute), false)
                });
            }

            return asm;
        }

        public Type GetClass(string fullyQualifiedClassName)
        {
            var t = AssemblyReference.GetType(fullyQualifiedClassName, false, false);

            if (t == null)
                throw new Exception("The class named: [" + fullyQualifiedClassName + "] could not be found.");

            return t;
        }
    }
}
