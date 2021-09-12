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
        VBNet = 1
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
        SerializeToCsv = 16,
        SerializeFromCsv = 32,
        SerializeToJson = 64,
        SerializeFromJson = 128,
        RepoStatic = 256,
        RepoDynamic = 512,
        RepoBulkCopy = 1024,
        RepoDapper = 2048,
        RepoEfFluentApi = 4096
    }
}
