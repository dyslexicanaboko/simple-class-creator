using SimpleClassCreator.DataAccess;
using SimpleClassCreator.Models;
using SimpleClassCreator.Services.CodeFactory;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace SimpleClassCreator.Services
{
    public class QueryToClassService 
        : IQueryToClassService
    {
        private readonly IQueryToClassRepository _repository;

        public QueryToClassService(IQueryToClassRepository repository)
        {
            _repository = repository;
        }

        public StringBuilder GenerateClass(ClassParameters parameters)
        {
            var p = parameters;

            _repository.ChangeConnectionString(p.ConnectionString);

            DotNetLanguage motif;

            if (p.LanguageType == CodeType.CSharp)
                motif = new CSharpLanguage(p.ClassName, p.IncludeSerializableAttribute, p.BuildOutClassProperties);
            else
                motif = new VbDotNetLanguage(p.ClassName, p.IncludeSerializableAttribute);

            if (parameters.IncludeNamespace)
                motif.NamespaceName = parameters.Namespace;

            motif.InitializeMotifValues();

            StringBuilder sb = GenerateClass(motif, p);

            if (p.SaveAsFile)
                WriteClassToFile(p, sb);

            return sb;
        }

        public StringBuilder GenerateGridViewColumns(ClassParameters parameters)
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

            return sb;
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
        private StringBuilder GenerateClass(DotNetLanguage language, ClassParameters parameters)
        {
            var p = parameters;

            //primaryKey = GetPrimaryKeyColumn(p.TableQuery);
            var sqlQuery = p.SourceType == SourceTypeEnum.TableName ? ("SELECT TOP 0 * FROM " + p.ClassSource) : p.ClassSource;

            var dt = _repository.GetSchema(sqlQuery);

            var ins = new ClassInstructions();
            
            if (language.IncludeSerializableAttribute)
            {
                ins.Namespaces.Add("System.Runtime.Serialization");
                ins.Attributes.Add(language.DataContract);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                var prop = new ClassMemberStrings(dc, p.LanguageType, p.MemberPrefix);

                ins.Properties.Add(prop);
            }

            return null;
        }

        /// <summary>
        /// Manufacture the physical C# or VB.Net code file
        /// </summary>
        /// <param name="fileName">The name of the file, including the file extension</param>
        /// <param name="sb">The StringBuilder that contains the generated code</param>
        private void WriteClassToFile(ClassParameters p, StringBuilder sb)
        {
            string fullFilePath = Path.Combine(p.Filepath, p.Filename);

            using (StreamWriter sw = new StreamWriter(fullFilePath, false))
            {
                sw.Write(sb.ToString());
            }

            Console.WriteLine("{0} Characters Written to {1}", sb.Length, fullFilePath);
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
