using System;

namespace SimpleClassCreator.Lib
{
    public enum SourceSqlType
    {
        Query,
        TableName
    }

    public enum CodeType
    {
        CSharp = 0,
        [Obsolete("Looking to get rid of this option and therefore this Enum all together. VB sucks.")]
        VBNet = 1,
        JavaScript = 2,
        TypeScript = 3
    }

    [Flags]
    public enum TableQueryQualifiers
    {
        None = 0,
        Table = 1,
        Schema = 2,
        Database = 4,
        LinkedServer = 8
    }

    [Flags]
    public enum ClassServices
    {
        None = 0,
        CloneEntityToModel = 1,
        CloneModelToEntity = 2,
        CloneInterfaceToEntity = 4,
        CloneInterfaceToModel = 8,
        SerializeCsv = 16,
        SerializeJson = 32,
        RepoStatic = 64,
        RepoDynamic = 128,
        RepoBulkCopy = 512,
        RepoDapper = 1024,
        RepoEfFluentApi = 2048
    }

    [Flags]
    public enum ClassRepositories
    {
        None = 0,
        StaticStatements = 1,
        Dapper = 2
    }
}
