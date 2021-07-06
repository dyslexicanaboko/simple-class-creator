using SimpleClassCreator.Models;

namespace SimpleClassCreator.Services
{
    public interface IClassService
    {
        TableQuery ParseTableName(string tableNameQuery);

        string FormatTableQuery(TableQuery tableQuery, TableQueryQualifiers qualifiers = TableQueryQualifiers.Schema | TableQueryQualifiers.Table);
    }
}
