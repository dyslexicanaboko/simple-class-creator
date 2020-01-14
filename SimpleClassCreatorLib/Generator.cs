using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using SimpleClassCreator.DataAccess;
using SimpleClassCreator.DTO;

namespace SimpleClassCreator
{
    public enum SourceTypeEnum
    { 
        Query,
        TableName
    }

    public static class Generator
    {
        /// <summary>
        /// Generate a class from this table name
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="language"></param>
        public static void Execute(string tableName, CodeType language)
        {
            Execute(tableName, language, string.Empty);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="language"></param>
        /// <param name="memberPrefix"></param>
        public static void Execute(string tableName, CodeType language, string memberPrefix)
        {
            Execute(tableName, language, memberPrefix, false);      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="language"></param>
        /// <param name="memberPrefix"></param>
        /// <param name="includeWCFTags"></param>
        public static void Execute(string tableName, CodeType language, string memberPrefix, bool includeWCFTags)
        {
            Execute(tableName, language, memberPrefix, includeWCFTags, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="language"></param>
        /// <param name="memberPrefix"></param>
        /// <param name="includeWCFTags"></param>
        /// <param name="buildOutProperties"></param>
        public static void Execute(string tableName, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties)
        { 
            string className = tableName;
            
            string sqlQuery = "SELECT TOP 1 * FROM " + tableName;

            Execute(sqlQuery, className, language, memberPrefix, includeWCFTags, buildOutProperties, tableName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="className"></param>
        /// <param name="language"></param>
        public static void Execute(string sqlQuery, string className, CodeType language)
        {
            Execute(sqlQuery, className, language, string.Empty, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="className"></param>
        /// <param name="language"></param>
        /// <param name="memberPrefix"></param>
        public static void Execute(string sqlQuery, string className, CodeType language, string memberPrefix)
        {
            Execute(sqlQuery, className, language, memberPrefix, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="className"></param>
        /// <param name="language"></param>
        /// <param name="memberPrefix"></param>
        /// <param name="includeWCFTags"></param>
        public static void Execute(string sqlQuery, string className, CodeType language, string memberPrefix, bool includeWCFTags)
        {
            Execute(sqlQuery, className, language, memberPrefix, includeWCFTags, false);        
        }
       

        /// <summary>
        /// Generate code for a SQL Query (Virtual Table) that cannot be mapped to a single table in your database
        /// </summary>
        /// <param name="sqlQuery">The query that this class will be derived from</param>
        /// <param name="className">Name this class since it cannot be mapped to a table in the database</param>
        /// <param name="language">C# or VB.Net</param>
        /// <param name="memberPrefix">Prefix for the private members of your class - can be left blank</param>
        /// <param name="buildOutProperties">Only relevant for C# - true: don't use the get; set; short hand for properties</param>
        /// <param name="includeWCFTags">Include all necessary WCF tags to make this class serializable.</param>
        /// <param name="tableName">Optional - Include the table name if CRUD is to be generated</param>
        public static void Execute(string sqlQuery, string className, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties, string tableName = null)
        {
            Execute(new ClassParameters() 
            { 
                ClassSource = sqlQuery,
                LanguageType = language,
                MemberPrefix = memberPrefix,
                ClassName = className,
                IncludeWcfTags = includeWCFTags,
                BuildOutClassProperties = buildOutProperties
            });
        }

        public static StringBuilder Execute(ClassParameters parameters)
        {
            ClassParameters p = parameters;

            DotNetLanguage motif;

            if (p.LanguageType == CodeType.CSharp)
                motif = new CSharpLanguage(p.ClassName, p.IncludeWcfTags, p.BuildOutClassProperties);
            else
                motif = new VBDotNetLanguage(p.ClassName, p.IncludeWcfTags);

            if (parameters.IncludeNamespace)
                motif.NamespaceName = parameters.Namespace;

            motif.InitializeMotifValues();

            StringBuilder sb = GenerateClass(motif, p);

            if (p.SaveAsFile)
                WriteClassToFile(p, sb);

            return sb;
        }

        public static StringBuilder GenerateGridViewColumns(ClassParameters parameters)
        {
            StringBuilder sb = new StringBuilder();

            ClassParameters p = parameters;

            #region Make this reusable
            string tableName, primaryKey, sqlQuery;

            if (p.SourceType == SourceTypeEnum.TableName)
            {
                tableName = p.ClassSource;
                primaryKey = GetPrimaryKeyColumn(p.ConnectionString, tableName);
                sqlQuery = "SELECT TOP 1 * FROM " + p.ClassSource;
            }
            else
            {
                tableName = null;
                primaryKey = null;
                sqlQuery = p.ClassSource;
            }
            #endregion

            DataTable dt = GetSchema(p.ConnectionString, sqlQuery);

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

        public static bool IsNumber(Type targetType)
        {
            return targetType.Equals(typeof(Int32)) ||
                targetType.Equals(typeof(Byte)) ||
                targetType.Equals(typeof(Int16)) ||
                targetType.Equals(typeof(Double)) ||
                targetType.Equals(typeof(Decimal));
        }

        /// <summary>
        /// The main internal method that orchestrates the code generation for the provided parameters
        /// </summary>
        /// <returns>The generated class code as a StringBuilder</returns>
        private static StringBuilder GenerateClass(DotNetLanguage motif, ClassParameters parameters)
        {
            ClassParameters p = parameters;

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
                primaryKey = GetPrimaryKeyColumn(p.ConnectionString, tableName);
                sqlQuery = "SELECT TOP 1 * FROM " + p.ClassSource;
            }
            else
            {
                tableName = null;
                primaryKey = null;
                sqlQuery = p.ClassSource;
            }

            DataTable dt = GetSchema(p.ConnectionString, sqlQuery);

            DotNetLanguage.MemberInfo info = null;

            //Using Statements
            sb.Append(motif.Using).Append(" System").Append(motif.LineTerminator);
            sb.Append(motif.Using).Append(" System.Collections.Generic").Append(motif.LineTerminator);
            sb.Append(motif.Using).Append(" System.Data").Append(motif.LineTerminator);
            sb.Append(motif.Using).Append(" System.Text").Append(motif.LineTerminator);

            if (motif.IncludeWCFTags)
                sb.Append(motif.Using).Append(" System.Runtime.Serialization").Append(motif.LineTerminator);
            
            sb.Append(Environment.NewLine);
            
            //Open the Namespace
            if (parameters.IncludeNamespace)
                sb.Append(motif.OpenNamespace);

            //Add the [DataContract] attribute
            if (motif.IncludeWCFTags)
                sb.Append(motif.DataContract);

            //Open the Class
            sb.Append(motif.OpenClass);
            sb.Append(motif.EmptyConstructor);
            sb.Append(motif.CreateRegion("Properties"));

            //Data Collection and Property Generation
            foreach (DataColumn dc in dt.Columns)
            {
                info = new DotNetLanguage.MemberInfo(dc, p.LanguageType, p.MemberPrefix);

                //DataColumn as Property
                motif.CreateProperty(sbProperties, info);

                //Object Generation Code
                sbObjectGen.Append("obj.").Append(info.Property).Append(" = ").Append(info.ConvertTo).Append(motif.DataRowGet("dr", dc.ColumnName)).Append(")").Append(motif.LineTerminator);

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
        private static void WriteClassToFile(ClassParameters p, StringBuilder sb)
        {
            string fullFilePath = Path.Combine(p.Filepath, p.Filename);

            using (StreamWriter sw = new StreamWriter(fullFilePath, false))
            {
                sw.Write(sb.ToString());    
            }

            Console.WriteLine("{0} Characters Written to {1}", sb.Length, fullFilePath);
        }

        /// <summary>
        /// Get the Schema from the database for the provided query
        /// </summary>
        /// <param name="sqlQuery">The target virtual table schema</param>
        /// <returns>DataTable containing the schema</returns>
        private static DataTable GetSchema(string connectionString, string sqlQuery)
        {
            return new GeneratorDAL(connectionString).GetSchema(sqlQuery);
        }

        /// <summary>
        /// Get the Primary Key Column Name from the provided table
        /// </summary>
        /// <param name="tableName">The name of the target table</param>
        /// <returns>Primary Key Column Name</returns>
        private static string GetPrimaryKeyColumn(string connectionString, string tableName)
        {
            return new GeneratorDAL(connectionString).GetPrimaryKeyColumn(tableName);
        }

        private static void TrimEnd(StringBuilder sb, string pattern)
        {
            sb = sb.Remove(sb.Length - pattern.Length, pattern.Length);
        }
    }
}
