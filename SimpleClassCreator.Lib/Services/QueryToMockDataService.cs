using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Lib.Services.Generators;
using System.Data;
using System.Linq;

namespace SimpleClassCreator.Lib.Services
{
    public interface IQueryToMockDataService
    {
        string GetEntity(QueryToMockDataParameters parameters);

        string GetMockData(QueryToMockDataParameters parameters);
    }

    public class QueryToMockDataService
        : ClassMetaDataBase, IQueryToMockDataService
    {
        private readonly IQueryToClassRepository _repository;
        private ClassInstructions _instructions;

        public QueryToMockDataService(IQueryToClassRepository repository, IGeneralDatabaseQueries genericDatabaseQueries)
            : base(repository, genericDatabaseQueries)
        {
            _repository = repository;
        }

        public string GetEntity(QueryToMockDataParameters parameters)
        {
            var p = parameters;

            _repository.ChangeConnectionString(p.ConnectionString);

            //Get the meta data needed about the entity
            var instructions = GetInstructions(p);

            var generator = new ClassEntitySimpleGenerator(instructions);

            //Generate the string representation of the class for preview
            var res = generator.FillTemplate();

            return res.Contents;
        }

        public string GetMockData(QueryToMockDataParameters parameters)
        {
            //Generate the mock data constructs using the entity for as much data as is requested.
            //Most of this is going to be contained inside of this service because there is no other way
            var p = parameters;

            _repository.ChangeConnectionString(p.ConnectionString);
            
            _genericDatabaseQueries.ChangeConnectionString(p.ConnectionString);

            //Get the meta data needed about the entity
            var instructions = GetInstructions(p);

            var dt = GetRowData(p.SourceSqlType, p.SourceSqlText);

            var generator = new ClassEntitySimpleGenerator(instructions);

            //Generate the string representation of the class for preview
            var res = generator.FillMockDataTemplate(dt);

            return res.Contents;
        }

        private ClassInstructions GetInstructions(QueryToMockDataParameters parameters)
        {
            if (_instructions != null) return _instructions;

            var p = parameters;

            var schema = GetSchema(p.SourceSqlType, p.SourceSqlText, p.TableQuery);

            var ins = new ClassInstructions();

            ins.ClassEntityName = p.ClassEntityName;
            ins.TableQuery = p.TableQuery;
            ins.Properties = schema.ColumnsAll.Select(x => new ClassMemberStrings(x)).ToList();

            _instructions = ins;

            return ins;
        }
    }
}
