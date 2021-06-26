using SimpleClassCreator.Models;
using SimpleClassCreator.Services.CodeFactory;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
                motif = new CSharpLanguage(p.ClassName, p.IncludeWcfTags, p.BuildOutClassProperties);
            else
                motif = new VbDotNetLanguage(p.ClassName, p.IncludeWcfTags);

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

            var sql = p.SourceType == SourceTypeEnum.TableName ? ("SELECT TOP 1 * FROM " + p.ClassSource) : p.ClassSource;

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
        private StringBuilder GenerateClass(DotNetLanguage motif, ClassParameters parameters)
        {
            var p = parameters;

            StringBuilder sb = new StringBuilder(),
                          sbColumns = new StringBuilder(),
                          sbProperties = new StringBuilder(),
                          sbObjectGen = new StringBuilder(),
                          sbUpdate = new StringBuilder(),
                          sbInsert = new StringBuilder();

            string tableName, primaryKey, sqlQuery;

            if (p.SourceType == SourceTypeEnum.TableName)
            {
                tableName = p.ClassSource;
                primaryKey = GetPrimaryKeyColumn(p.TableQuery);
                sqlQuery = "SELECT TOP 1 * FROM " + p.ClassSource;
            }
            else
            {
                tableName = null;
                primaryKey = null;
                sqlQuery = p.ClassSource;
            }

            var dt = _repository.GetSchema(sqlQuery);

            //Using Statements
            sb.Append(motif.Using).Append(" System").Append(motif.LineTerminator);
            sb.Append(motif.Using).Append(" System.Collections.Generic").Append(motif.LineTerminator);
            sb.Append(motif.Using).Append(" System.Data").Append(motif.LineTerminator);
            sb.Append(motif.Using).Append(" System.Text").Append(motif.LineTerminator);

            if (motif.IncludeSerializableAttribute)
                sb.Append(motif.Using).Append(" System.Runtime.Serialization").Append(motif.LineTerminator);

            sb.Append(Environment.NewLine);

            //Open the Namespace
            if (parameters.IncludeNamespace)
                sb.Append(motif.OpenNamespace);

            //Add the [DataContract] attribute
            if (motif.IncludeSerializableAttribute)
                sb.Append(motif.DataContract);

            //Open the Class
            sb.Append(motif.OpenClass);
            sb.Append(motif.EmptyConstructor);
            sb.Append(motif.CreateRegion("Properties"));

            //Data Collection and Property Generation
            foreach (DataColumn dc in dt.Columns)
            {
                var info = new DotNetLanguage.MemberInfo(dc, p.LanguageType, p.MemberPrefix);

                //DataColumn as Property
                motif.CreateProperty(sbProperties, info);

                //Object Generation Code
                sbObjectGen.Append("obj.").Append(info.Property).Append(" = ");

                var dr = motif.DataRowGet("dr", dc.ColumnName);
                var conv = info.ConvertTo + dr + ")";

                if (info.IsNullable)
                {
                    sbObjectGen.Append(dr).Append(" == DBNull.Value ? null : new ").Append(info.SystemType).Append("(").Append(conv).Append(")");
                }
                else
                {
                    sbObjectGen.Append(conv);
                }

                sbObjectGen.Append(motif.LineTerminator);

                //I don't remember why I did any of this:
                if (tableName != null && info.ColumnName != primaryKey)
                {
                    //Column CSV
                    sbColumns.Append(dc.ColumnName).Append(", ");

                    //Update Statement Code
                    sbUpdate.Append("sb.Append(\"").Append(dc.ColumnName).Append("\").Append(\" = \").Append(").Append(info.StringValue).Append(").Append(\",\")").Append(motif.LineTerminator);

                    //Insert Statement Code
                    sbInsert.Append("sb.Append(").Append(info.StringValue).Append(").Append(\",\")").Append(motif.LineTerminator);
                }
            }

            //This section can only be performed if this is a single table that this code is being generated from.
            //Virtual Tables do not qualify because they won't have primary keys
            if (tableName != null)
            {
                //Trim the trailing patterns from these 
                TrimEnd(sbColumns, "\",\" ");
                TrimEnd(sbUpdate, ".Append(\",\")" + motif.LineTerminator);
                TrimEnd(sbInsert, ".Append(\",\")" + motif.LineTerminator);
            }

            //Append the Class Private Members and Public Properties
            sb.Append(sbProperties);
            sb.Append(motif.EndRegion);

            //Object Generation Method
            motif.CreateObjectGenerationMethod(sb, sbObjectGen.ToString());

            if (tableName != null)
            {
                //Update Method
                if (dt.Columns[primaryKey] != null)
                    motif.CreateUpdateMethod(sb, sbUpdate.ToString(), new DotNetLanguage.MemberInfo(dt.Columns[primaryKey], p.LanguageType, p.MemberPrefix));

                //Insert Method
                motif.CreateInsertMethod(sb, sbColumns.ToString(), sbInsert.ToString());

                //AddString Methods
                motif.CreateAddStringMethods(sb);
            }

            sb.Append(motif.CloseClass);

            if (parameters.IncludeNamespace)
                sb.Append(motif.CloseNamespace);

            return sb;
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

        private StringBuilder TrimEnd(StringBuilder sb, string pattern)
        {
            return sb.Remove(sb.Length - pattern.Length, pattern.Length);
        }

        public TableQuery ParseTableName(string tableNameQuery)
        {
            var q = Regex.Replace(tableNameQuery, @"\s+", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty);

            var arr = q.Split('.');

            var tbl = new TableQuery();

            switch (arr.Length)
            {
                //Table
                case 1:
                    tbl.Table = arr[0];
                    break;
                //Schema.Table
                case 2:
                    tbl.Schema = arr[0];
                    tbl.Table = arr[1];
                    break;
                //Database.Schema.Table
                case 3:
                    tbl.Database = arr[0];
                    tbl.Schema = arr[1];
                    tbl.Table = arr[2];
                    break;
                //LinkedServer.Database.Schema.Table
                case 4:
                    tbl.LinkedServer = arr[0];
                    tbl.Database = arr[1];
                    tbl.Schema = arr[2];
                    tbl.Table = arr[3];
                    break;
            }

            return tbl;
        }
    }
}
