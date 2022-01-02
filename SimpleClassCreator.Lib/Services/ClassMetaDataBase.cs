using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using System.Data;

namespace SimpleClassCreator.Lib.Services
{
    public abstract class ClassMetaDataBase
    {
        protected readonly IQueryToClassRepository _queryToClassRepository;
        protected readonly IGeneralDatabaseQueries _genericDatabaseQueries;

        protected ClassMetaDataBase(IQueryToClassRepository repository, IGeneralDatabaseQueries genericDatabaseQueries)
        {
            _queryToClassRepository = repository;
            
            _genericDatabaseQueries = genericDatabaseQueries;
        }

        protected virtual SchemaQuery GetSchema(SourceSqlType sourceSqlType, string sourceSqlText, TableQuery tableQuery)
        {
            //primaryKey = GetPrimaryKeyColumn(p.TableQuery); //This is specific to the repos
            var selector = sourceSqlType == SourceSqlType.TableName ? "SELECT * FROM " : string.Empty;

            var sqlQuery = $"SET FMTONLY ON; {selector}{sourceSqlText}; SET FMTONLY OFF;";

            var schema = _queryToClassRepository.GetSchema(tableQuery, sqlQuery);

            return schema;
        }

        protected virtual DataTable GetRowData(SourceSqlType sourceSqlType, string sourceSqlText, int top = 5)
        {
            var selector = sourceSqlType == SourceSqlType.TableName ? $"SELECT TOP({top}) * FROM " : string.Empty;

            var sqlQuery = $"{selector}{sourceSqlText}";

            var dt = _genericDatabaseQueries.GetRowData(sqlQuery);

            return dt;
        }
    }
}
