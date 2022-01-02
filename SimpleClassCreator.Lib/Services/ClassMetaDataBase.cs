using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services
{
    public abstract class ClassMetaDataBase
    {
        protected readonly IQueryToClassRepository _queryToClassRepository;

        protected ClassMetaDataBase(IQueryToClassRepository repository)
        {
            _queryToClassRepository = repository;
        }

        protected virtual SchemaQuery GetSchema(SourceSqlType sourceSqlType, string sourceSqlText, TableQuery tableQuery)
        {
            //primaryKey = GetPrimaryKeyColumn(p.TableQuery); //This is specific to the repos
            var selector = sourceSqlType == SourceSqlType.TableName ? "SELECT * FROM " : string.Empty;

            var sqlQuery = $"SET FMTONLY ON; {selector}{sourceSqlText}; SET FMTONLY OFF;";

            var schema = _queryToClassRepository.GetSchema(tableQuery, sqlQuery);

            return schema;
        }
    }
}
