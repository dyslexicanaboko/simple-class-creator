using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Lib.Services.Generators;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace SimpleClassCreator.Lib.Services
{
    public class QueryToClassService 
        : IQueryToClassService
    {
        private readonly IQueryToClassRepository _repository;

        public QueryToClassService(IQueryToClassRepository repository)
        {
            _repository = repository;
        }

        public string GenerateClass(ClassParameters parameters)
        {
            var p = parameters;

            _repository.ChangeConnectionString(p.ConnectionString);

            DotNetLanguage motif;

            if (p.LanguageType == CodeType.CSharp)
                motif = new CSharpLanguage(p.ClassName);
            else
                motif = new VbDotNetLanguage(p.ClassName);

            motif.NamespaceName = parameters.Namespace;

            motif.InitializeMotifValues();

            var content = GenerateClass(motif, p);

            if (p.SaveAsFile)
                WriteClassToFile(p, content);

            return content;
        }

        public string GenerateGridViewColumns(ClassParameters parameters)
        {
            var p = parameters;
            
            _repository.ChangeConnectionString(p.ConnectionString);

            var sql = p.SourceType == SourceTypeEnum.TableName ? ("SELECT TOP 0 * FROM " + p.ClassSource) : p.ClassSource;

            var dt = _repository.GetSchema(sql);

            var sb = new StringBuilder();

            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append("<asp:BoundField HeaderText=\"")
                  .Append(dc.ColumnName)
                  .Append("\" DataField=\"")
                  .Append(dc.ColumnName)
                  .AppendLine("\">")
                  .AppendLine("<HeaderStyle HorizontalAlign=\"" + (IsNumber(dc.DataType) ? "Right" : "Left") + "\" />")
                  .AppendLine("</asp:BoundField>");
            }

            var content = sb.ToString();

            return content;
        }

        private bool IsNumber(Type targetType)
        {
            return targetType.Equals(typeof(int)) ||
                targetType.Equals(typeof(byte)) ||
                targetType.Equals(typeof(short)) ||
                targetType.Equals(typeof(double)) ||
                targetType.Equals(typeof(decimal));
        }

        /// <summary>
        /// The main internal method that orchestrates the code generation for the provided parameters
        /// </summary>
        /// <returns>The generated class code as a StringBuilder</returns>
        private string GenerateClass(DotNetLanguage language, ClassParameters parameters)
        {
            var p = parameters;

            //primaryKey = GetPrimaryKeyColumn(p.TableQuery);
            var sqlQuery = p.SourceType == SourceTypeEnum.TableName ? ("SELECT TOP 0 * FROM " + p.ClassSource) : p.ClassSource;

            var dt = _repository.GetSchema(sqlQuery);

            var ins = new ClassInstructions();

            ins.ClassName = p.ClassName;
            ins.Namespace = p.Namespace;

            if (language.IncludeSerializableAttribute)
            {
                ins.Namespaces.Add("System.Runtime.Serialization");
                ins.ClassAttributes.Add(language.DataContract);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                var prop = new ClassMemberStrings(dc, p.LanguageType);

                //Add the system namespace if any of the properties require it
                if (prop.InSystemNamespace) ins.AddNamespace("System");

                ins.Properties.Add(prop);
            }

            //How to inject this without having to do away with the constructor access?
            var svc = new ModelGenerator(ins);

            var content = svc.FillTemplate();

            return content;
        }

        /// <summary>
        /// Manufacture the physical C# or VB.Net code file
        /// </summary>
        /// <param name="fileName">The name of the file, including the file extension</param>
        /// <param name="sb">The StringBuilder that contains the generated code</param>
        private void WriteClassToFile(ClassParameters p, string content)
        {
            var fullFilePath = Path.Combine(p.Filepath, p.Filename);

            File.WriteAllText(fullFilePath, content);

            Console.WriteLine($"{content.Length} Characters Written to {fullFilePath}");
        }

        /// <summary>
        /// Get the Primary Key Column Name from the provided table
        /// </summary>
        /// <param name="tableQuery">The name of the target table</param>
        /// <returns>Primary Key Column Name</returns>
        private string GetPrimaryKeyColumn(TableQuery tableQuery)
        {
            if (string.IsNullOrEmpty(tableQuery.Schema) || string.IsNullOrEmpty(tableQuery.Table)) return "PK";

            return _repository.GetPrimaryKeyColumn(tableQuery);
        }
    }
}
