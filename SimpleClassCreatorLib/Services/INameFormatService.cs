using SimpleClassCreator.Models;

namespace SimpleClassCreator.Services
{
    public interface INameFormatService
    {
        TableQuery ParseTableName(string tableNameQuery);

        string GetClassName(TableQuery tableQuery);

        string FormatTableQuery(string tableQuery, TableQueryQualifiers qualifiers = TableQueryQualifiers.Schema | TableQueryQualifiers.Table);

        string FormatTableQuery(TableQuery tableQuery, TableQueryQualifiers qualifiers = TableQueryQualifiers.Schema | TableQueryQualifiers.Table);
    }
}
