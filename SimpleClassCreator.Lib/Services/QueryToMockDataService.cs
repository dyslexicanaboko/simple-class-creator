using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Lib.Services.Generators;
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
     
        public QueryToMockDataService(IQueryToClassRepository repository)
            : base(repository)
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

            return null;
        }

        private ClassInstructions GetInstructions(QueryToMockDataParameters parameters)
        {
            var p = parameters;

            var schema = GetSchema(p.SourceSqlType, p.SourceSqlText, p.TableQuery);

            var ins = new ClassInstructions();

            ins.ClassEntityName = p.ClassEntityName;
            ins.TableQuery = p.TableQuery;
            ins.Properties = schema.ColumnsAll.Select(x => new ClassMemberStrings(x)).ToList();

            return ins;
        }
    }
}
