using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Lib.Services.Generators;
using System;
using System.Collections.Generic;
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

            var baseInstructions = GetInstructions(parameters);

            var rClasses = GenerateClasses(p, baseInstructions);

            var rServices = GenerateServices(p, baseInstructions);

            var rRepositories = GenerateRepositories(p, baseInstructions);

            var lst = new List<GeneratedResult>(
                rClasses.Count + 
                rServices.Count +
                rRepositories.Count);
            
            lst.AddRange(rClasses);
            lst.AddRange(rServices);
            lst.AddRange(rRepositories);

            //Writing to files will be handled again later
            //if (p.SaveAsFile)
            //    WriteClassToFile(p, content);

            return lst;
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
        private IList<GeneratedResult> GenerateClasses(QueryToClassParameters parameters, ClassInstructions baseInstructions)
        {
            var co = parameters.ClassOptions;

            var lst = new List<GeneratedResult>();

            var interfaceName = string.Empty;

            if (co.GenerateInterface)
            {
                interfaceName = baseInstructions.InterfaceName;

                var ins = baseInstructions.Clone();
                ins.ClassEntityName = ins.InterfaceName;

                var svc = new ClassInterfaceGenerator(ins);

                lst.Add(svc.FillTemplate());
            }

            if (co.GenerateEntity)
            {
                var ins = baseInstructions.Clone();
                ins.ClassEntityName = co.ClassEntityName;
                ins.InterfaceName = interfaceName;
                ins.IsPartial = co.GenerateEntityIEquatable || co.GenerateEntityIComparable;

                var svc = new ClassEntityGenerator(ins);

                lst.Add(svc.FillTemplate());

                if (co.GenerateEntityIEquatable)
                {
                    var insSub = baseInstructions.Clone();
                    insSub.ClassEntityName = co.ClassEntityName;

                    var svcSub = new ClassEntityIEquatableGenerator(insSub);

                    lst.Add(svcSub.FillTemplate());
                }

                if (co.GenerateEntityIComparable)
                {
                    var insSub = baseInstructions.Clone();
                    insSub.ClassEntityName = co.ClassEntityName;

                    var svcSub = new ClassEntityIComparableGenerator(insSub);

                    lst.Add(svcSub.FillTemplate());
                }

                if (co.GenerateEntityEqualityComparer)
                {
                    var insSub = baseInstructions.Clone();
                    insSub.ClassEntityName = co.ClassEntityName;

                    var svcSub = new ClassEntityEqualityComparerGenerator(insSub);

                    lst.Add(svcSub.FillTemplate());
                }
            }

            if (co.GenerateModel)
            {
                var ins = baseInstructions.Clone();
                ins.ClassEntityName = co.ClassModelName;
                ins.InterfaceName = interfaceName;

                var svc = new ClassModelGenerator(ins);

                lst.Add(svc.FillTemplate());
            }

            return lst;
        }

        private IList<GeneratedResult> GenerateServices(QueryToClassParameters parameters, ClassInstructions baseInstructions)
        {
            if (parameters.ClassServices == ClassServices.None) return new List<GeneratedResult>(0);

            var services = parameters.ClassServices;

            var lst = new List<GeneratedResult>();

            if (services.HasFlag(ClassServices.SerializeCsv))
            {
                var svc = new ServiceSerializationCsvGenerator(baseInstructions);

                lst.Add(svc.FillTemplate());
            }

            if (services.HasFlag(ClassServices.SerializeJson))
            {
                var svc = new ServiceSerializationJsonGenerator(baseInstructions);

                lst.Add(svc.FillTemplate());
            }

            if (services.HasFlag(ClassServices.CloneModelToEntity) ||
                services.HasFlag(ClassServices.CloneEntityToModel) ||
                services.HasFlag(ClassServices.CloneInterfaceToEntity) ||
                services.HasFlag(ClassServices.CloneInterfaceToModel))
            {
                var svc = new ServiceCloningGenerator(baseInstructions, services);

                lst.Add(svc.FillTemplate());
            }

            return lst;
        }

        private IList<GeneratedResult> GenerateRepositories(QueryToClassParameters parameters, ClassInstructions baseInstructions)
        {
            if (parameters.ClassRepositories  == ClassRepositories.None) return new List<GeneratedResult>(0);

            var repositories = parameters.ClassRepositories;

            var lst = new List<GeneratedResult>();

            if (repositories.HasFlag(ClassRepositories.StaticStatements))
            {
                var svc = new RepositoryStaticGenerator(baseInstructions);

                lst.Add(svc.FillTemplate());
            }

            if (repositories.HasFlag(ClassRepositories.Dapper))
            {
                //var svc = new RepositoryDapperGenerator(baseInstructions);

                //lst.Add(svc.FillTemplate());
            }

            return lst;
        }

        private ClassInstructions GetInstructions(QueryToClassParameters parameters)
        {
            var p = parameters;
         
            //primaryKey = GetPrimaryKeyColumn(p.TableQuery); //This is specific to the repos
            var sqlQuery = p.SourceSqlType == SourceSqlType.TableName ? ("SET FMTONLY ON; SELECT * FROM " + p.SourceSqlText + "; SET FMTONLY OFF;") : p.SourceSqlText;

            var schema = _repository.GetSchema(p.TableQuery, sqlQuery);

            var ins = new ClassInstructions();

            ins.Namespace = p.Namespace;
            ins.EntityName = p.ClassOptions.EntityName;
            ins.ClassEntityName = p.ClassOptions.ClassEntityName;
            ins.ClassModelName = p.ClassOptions.ClassModelName;
            ins.InterfaceName = "I" + (string.IsNullOrEmpty(p.ClassOptions.ClassEntityName) ? p.ClassOptions.ClassModelName : p.ClassOptions.ClassEntityName);
            ins.TableQuery = p.TableQuery;

            foreach (var sc in schema.ColumnsAll)
            {
                var prop = new ClassMemberStrings(sc, p.LanguageType);

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
