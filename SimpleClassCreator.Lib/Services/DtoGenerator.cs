using SimpleClassCreator.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SimpleClassCreator.Lib.Services.CodeFactory;

namespace SimpleClassCreator.Lib.Services
{
    public class DtoGenerator 
        : IDtoGenerator
    {
        public DtoGenerator()
        {
            
        }
        
        private string AssemblyPath { get; set; }
        
        private string FileName { get; set; }
        
        public Assembly AssemblyReference { get; private set; }

        public void LoadAssembly(string assemblyPath)
        {
            AssemblyPath = assemblyPath;

            FileName = Path.GetFileName(assemblyPath);

            AssemblyReference = Assembly.LoadFile(AssemblyPath);
        }

        private AssemblyInfo GetEmptyAssemblyInfo()
        {
            var asm = new AssemblyInfo {Name = FileName};

            return asm;
        }

        public AssemblyInfo GetListOfClasses()
        {
            var asm = GetEmptyAssemblyInfo();

            var lst = AssemblyReference
                .GetTypes()
                .TakeWhile(x => x.IsClass)
                .Select(x => x.FullName);

            asm.Add(lst);

            return asm;
        }

        public AssemblyInfo GetClassProperties(string className)
        {
            var t = GetTypeOrThrow(className);

            var asm = GetEmptyAssemblyInfo();

            var cInfo = asm.Add(className);

            foreach (var pi in t.GetProperties())
            {
                Console.WriteLine(pi.Name);

                var cms = new ClassMemberStrings(pi);

                cInfo.Properties.Add(new ClassProperty
                {
                    Name = pi.Name,
                    TypeName = cms.SystemTypeAlias,
                    IsSerializable = pi.PropertyType.IsDefined(typeof(SerializableAttribute), false)
                });
            }

            return asm;
        }

        public string MakeDto(string className, DtoMakerParameters parameters)
        {
            var p = parameters;

            var t = GetTypeOrThrow(className);

            if (t == null)
                return "Type cannot be null";

            var sb = new StringBuilder(); //Class

            var cn = t.Name + "Dto";

            sb.Append("public class ").AppendLine(cn);

            if (p.IncludeIEquatableOfTMethods) sb.Append("\t : IEquatable<").Append(cn).AppendLine(">");

            sb.AppendLine("{");

            var arrProperties = t.GetProperties();

            var lstPropertyNames = new List<string>(arrProperties.Length);

            foreach (var pi in arrProperties)
            {
                Console.WriteLine(pi.Name);

                lstPropertyNames.Add(pi.Name);

                var tp = pi.PropertyType;

                sb.Append("public ").Append("NOT IMPLEMENTED");

                sb.Append(" ").Append(pi.Name).AppendLine(" { get; set; } ").AppendLine();
            }

            sb.AppendLine();

            //Methods to include in the class
            if (p.IncludeTranslateMethod)
            {
                //var tm = GetTranslateMethod(t.Name, cn, lstPropertyNames);

                //sb.Append(tm);
            }

            if (p.IncludeCloneMethod)
            {
                //var cm = GetCloneMethod(cn, lstPropertyNames);

                //sb.Append(cm);
            }

            if (p.IncludeIEquatableOfTMethods)
            {
                //var eq = GetIEquatableOfTMethods(cn, lstPropertyNames);

                //sb.Append(eq);
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private Type GetTypeOrThrow(string className)
        {
            var t = AssemblyReference.GetType(className, false, false);

            if (t == null)
                throw new Exception("The class named: [" + className + "] could not be found.");

            return t;
        }

        #region Needs to be replaced
        //private StringBuilder GetTranslateMethod(string originalClassName, string dtoClassName, List<string> propertyNames)
        //{
        //    var cn = dtoClassName;

        //    var sb = new StringBuilder();

        //    //Open
        //    sb.Append("public ").Append(cn).Append(" Translate(").Append(originalClassName).AppendLine(" obj)");
        //    sb.AppendLine("{");
        //    sb.Append("var dto = new ").Append(cn).AppendLine("();");
        //    sb.AppendLine();

        //    //Properties
        //    propertyNames.ForEach(x => sb.Append("dto.").Append(x).Append(" = obj.").Append(x).AppendLine(";"));

        //    //Close
        //    sb.AppendLine("return dto;");
        //    sb.AppendLine("}");

        //    return sb;
        //}

        //private StringBuilder GetCloneMethod(string className, List<string> propertyNames)
        //{
        //    var cn = className;

        //    var sb = new StringBuilder();

        //    //Open
        //    sb.Append("public ").Append(cn).AppendLine(" Clone()");
        //    sb.AppendLine("{");
        //    sb.Append("var c = new ").Append(cn).AppendLine("();");
        //    sb.AppendLine();

        //    //Properties
        //    propertyNames.ForEach(x => sb.Append("c.").Append(x).Append(" = ").Append(x).AppendLine(";"));

        //    //Close
        //    sb.AppendLine("return c;");
        //    sb.AppendLine("}");

        //    return sb;
        //} 
        #endregion
    }
}