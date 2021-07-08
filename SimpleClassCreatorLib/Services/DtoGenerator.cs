using Microsoft.CSharp;
using SimpleClassCreator.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SimpleClassCreator.Services
{
    public class DtoGenerator 
        : IDtoGenerator
    {
        private readonly string _templatesPath;
        
        private readonly CSharpCodeProvider _compiler;

        public DtoGenerator()
        {
            _compiler = new CSharpCodeProvider();

            var dir = AppDomain.CurrentDomain.BaseDirectory;

            _templatesPath = Path.Combine(dir, "Templates");
        }

        
        private string AssemblyPath { get; set; }
        
        private string FileName { get; set; }
        
        public Assembly AssemblyReference { get; private set; }

        public void LoadAssembly(string assemblyPath)
        {
            AssemblyPath = assemblyPath;

            //BasePath = Path.GetDirectoryName(assemblyPath);

            FileName = Path.GetFileName(assemblyPath);

            AssemblyReference = Assembly.LoadFile(AssemblyPath);
        }

        public Type PrintClass(string className)
        {
            var t = AssemblyReference.GetType(className, false, false);

            if (t != null)
                Console.WriteLine(t.FullName);
            else
                Console.WriteLine("The Class Named: [" + className + "] could not be found (null returned).");

            return t;
        }

        public AssemblyInfo GetClassProperties(string className)
        {
            var asm = new AssemblyInfo();

            asm.Name = FileName;

            var t = PrintClass(className);

            if (t == null)
                return asm;

            var cInfo = asm.AddClass(className);

            foreach (var pi in t.GetProperties())
            {
                Console.WriteLine(pi.Name);

                cInfo.Properties.Add(new Models.PropertyInfo
                {
                    Name = pi.Name,
                    TypeName = GetTypeAsString(pi.PropertyType),
                    IsSerializable = pi.PropertyType.IsDefined(typeof(SerializableAttribute), false)
                });
            }

            return asm;
        }

        public string MakeDto(string className, ClassParameters parameters)
        {
            var p = parameters;

            var t = PrintClass(className);

            if (t == null)
                return "Type cannot be null";

            var sb = new StringBuilder(); //Class

            var cn = t.Name + "Dto";

            var wcfOk = p.IncludeSerializeablePropertiesOnly && p.IncludeSerializableAttribute;

            //Class declaration
            if (wcfOk) sb.AppendLine("[DataContract]");

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

                //Class properties
                if (wcfOk) sb.AppendLine("[DataMember]");

                sb.Append("public ").Append(GetTypeAsString(tp));

                sb.Append(" ").Append(pi.Name).AppendLine(" { get; set; } ").AppendLine();
            }

            sb.AppendLine();

            //Methods to include in the class
            if (p.IncludeTranslateMethod)
            {
                var tm = GetTranslateMethod(t.Name, cn, lstPropertyNames);

                sb.Append(tm);
            }

            if (p.IncludeCloneMethod)
            {
                var cm = GetCloneMethod(cn, lstPropertyNames);

                sb.Append(cm);
            }

            if (p.IncludeIEquatableOfTMethods)
            {
                var eq = GetIEquatableOfTMethods(cn, lstPropertyNames);

                sb.Append(eq);
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private StringBuilder GetIEquatableOfTMethods(string className, List<string> propertyNames)
        {
            var cn = className;

            var sb = new StringBuilder();

            var template = File.ReadAllText(Path.Combine(_templatesPath, "IEquatableOfT.cs"));

            sb.Append(template);

            var hc = new StringBuilder();
            var eq = new StringBuilder();

            propertyNames.ForEach(x =>
            {
                hc.Append("\t\t\t").Append(x).AppendLine(".GetHashCode() + ");

                eq.Append("\t\t\t").Append(x).Append(" == o.").Append(x).AppendLine(" &&");
            });

            //Remove trailing "+ \n\r" in order to terminate the statement
            hc.Remove(hc.Length - 5, 5);

            //Remove trailing " && \n\r" in order to terminate the statement
            eq.Remove(eq.Length - 5, 5);

            sb.Replace("%className%", cn)
              .Replace("%hashCode%", hc.ToString())
              .Replace("%equals%", eq.ToString());

            sb.AppendLine();

            return sb;
        }

        private StringBuilder GetTranslateMethod(string originalClassName, string dtoClassName, List<string> propertyNames)
        {
            var cn = dtoClassName;

            var sb = new StringBuilder();

            //Open
            sb.Append("public ").Append(cn).Append(" Translate(").Append(originalClassName).AppendLine(" obj)");
            sb.AppendLine("{");
            sb.Append("var dto = new ").Append(cn).AppendLine("();");
            sb.AppendLine();

            //Properties
            propertyNames.ForEach(x => sb.Append("dto.").Append(x).Append(" = obj.").Append(x).AppendLine(";"));

            //Close
            sb.AppendLine("return dto;");
            sb.AppendLine("}");

            return sb;
        }

        private StringBuilder GetCloneMethod(string className, List<string> propertyNames)
        {
            var cn = className;

            var sb = new StringBuilder();

            //Open
            sb.Append("public ").Append(cn).AppendLine(" Clone()");
            sb.AppendLine("{");
            sb.Append("var c = new ").Append(cn).AppendLine("();");
            sb.AppendLine();

            //Properties
            propertyNames.ForEach(x => sb.Append("c.").Append(x).Append(" = ").Append(x).AppendLine(";"));

            //Close
            sb.AppendLine("return c;");
            sb.AppendLine("}");

            return sb;
        }

        public string GetTypeAsString(Type target)
        {
            if (target.IsValueType && target.Name == "Nullable`1")
            {
                var typeT = target.GetGenericArguments();

                var typeAsString = GetTypeOutput(typeT[0]);

                var strNullableType = $"{typeAsString}?";

                return strNullableType;
            }

            var str = GetTypeOutput(target);

            return str;
        }

        private string GetTypeOutput(Type target)
        {
            var str = _compiler.GetTypeOutput(new CodeTypeReference(target));

            return str;
        }

        public void PrintClasses()
        {
            if (true) ;

            var i = 0;

            //This is a dangerous call for any large assemblies
            foreach (var type in AssemblyReference.GetTypes())
            {
                Console.WriteLine(type.FullName);

                i++;

                if (i >= 10)
                    break;
            }

            //Type type = asm.GetType("TestRunner");
        }
    }
}