using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Lib.Services.Generators;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

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

        public IList<GeneratedResult> Generate(QueryToClassParameters parameters)
        {
            var p = parameters;

            if (!p.HasElections) return null;

            _repository.ChangeConnectionString(p.ConnectionString);

            var results = GenerateClasses(p);

            //Writing to files will be handled again later
            //if (p.SaveAsFile)
            //    WriteClassToFile(p, content);

            return results;
        }

        #region Generate GridView
        //This is a relic of the past, not sure if I will continue to support this as it is just another template esssentially
        //public string GenerateGridViewColumns(QueryToClassParameters parameters)
        //{
        //    var p = parameters;

        //    _repository.ChangeConnectionString(p.ConnectionString);

        //    var sql = p.SourceSqlType == SourceSqlType.TableName ? ("SELECT TOP 0 * FROM " + p.SourceSqlText) : p.SourceSqlText;

        //    var dt = _repository.GetSchema(sql);

        //    var sb = new StringBuilder();

        //    foreach (DataColumn dc in dt.Columns)
        //    {
        //        sb.Append("<asp:BoundField HeaderText=\"")
        //          .Append(dc.ColumnName)
        //          .Append("\" DataField=\"")
        //          .Append(dc.ColumnName)
        //          .AppendLine("\">")
        //          .AppendLine("<HeaderStyle HorizontalAlign=\"" + (IsNumber(dc.DataType) ? "Right" : "Left") + "\" />")
        //          .AppendLine("</asp:BoundField>");
        //    }

        //    var content = sb.ToString();

        //    return content;
        //}
        #endregion

        /// <summary>
        /// The main internal method that orchestrates the code generation for the provided parameters
        /// </summary>
        /// <returns>The generated class code as a StringBuilder</returns>
        private IList<GeneratedResult> GenerateClasses(QueryToClassParameters parameters)
        {
            var co = parameters.ClassOptions;

            var lst = new List<GeneratedResult>();

            var baseInstructions = GetInstructions(parameters);

            string interfaceName = null;

            if (co.GenerateInterface)
            {
                //This is going to have a problem with the naming. I will have to deal with that later.
                interfaceName = "I" + co.EntityName;

                var ins = baseInstructions.Clone();
                ins.ClassName = interfaceName;

                var svc = new ClassInterfaceGenerator(ins);

                lst.Add(svc.FillTemplate());
            }

            if (co.GenerateEntity)
            {
                var ins = baseInstructions.Clone();
                ins.ClassName = co.EntityName;
                ins.InterfaceName = interfaceName;
                ins.IsPartial = co.GenerateEntityIEquatable || co.GenerateEntityIComparable;

                var svc = new ClassEntityGenerator(ins);

                lst.Add(svc.FillTemplate());

                if (co.GenerateEntityIEquatable)
                {
                    var insSub = baseInstructions.Clone();
                    insSub.ClassName = co.EntityName;

                    var svcSub = new ClassEntityIEquatableGenerator(insSub);

                    lst.Add(svcSub.FillTemplate());
                }

                if (co.GenerateEntityIComparable)
                {
                    var insSub = baseInstructions.Clone();
                    insSub.ClassName = co.EntityName;

                    var svcSub = new ClassEntityIComparableGenerator(insSub);

                    lst.Add(svcSub.FillTemplate());
                }

                if (co.GenerateEntityEqualityComparer)
                {
                    var insSub = baseInstructions.Clone();
                    insSub.ClassName = co.EntityName;

                    var svcSub = new ClassEntityEqualityComparerGenerator(insSub);

                    lst.Add(svcSub.FillTemplate());
                }
            }

            if (co.GenerateModel)
            {
                var ins = baseInstructions.Clone();
                ins.ClassName = co.ModelName;
                ins.InterfaceName = interfaceName;

                var svc = new ClassModelGenerator(ins);

                lst.Add(svc.FillTemplate());
            }

            return lst;
        }

        private ClassInstructions GetInstructions(QueryToClassParameters parameters)
        {
            var p = parameters;
         
            //primaryKey = GetPrimaryKeyColumn(p.TableQuery); //This is specific to the repos
            var sqlQuery = p.SourceSqlType == SourceSqlType.TableName ? ("SELECT TOP 0 * FROM " + p.SourceSqlText) : p.SourceSqlText;

            var dt = _repository.GetSchema(sqlQuery);

            var ins = new ClassInstructions();

            ins.ClassName = p.ClassOptions.EntityName;
            ins.Namespace = p.Namespace;

            foreach (DataColumn dc in dt.Columns)
            {
                var prop = new ClassMemberStrings(dc, p.LanguageType);

                //Add the system namespace if any of the properties require it
                if (prop.InSystemNamespace) ins.AddNamespace("System");

                ins.Properties.Add(prop);
            }

            return ins;
        }

        /// <summary>
        /// Manufacture the physical code file
        /// </summary>
        /// <param name="p">Generation parameters</param>
        /// <param name="content">Class contents</param>
        private void WriteClassToFile(QueryToClassParameters p, string content)
        {
            var fullFilePath = Path.Combine(p.FilePath, p.Filename);

            File.WriteAllText(fullFilePath, content);

            Console.WriteLine($"{content.Length} Characters Written to {fullFilePath}");
        }

        //This is specific to the repos
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

        //I don't remember what this was for. Will keep it around until later.
        private bool IsNumber(Type targetType)
        {
            return targetType == typeof(int) ||
                   targetType == typeof(byte) ||
                   targetType == typeof(short) ||
                   targetType == typeof(double) ||
                   targetType == typeof(decimal);
        }
    }
}
