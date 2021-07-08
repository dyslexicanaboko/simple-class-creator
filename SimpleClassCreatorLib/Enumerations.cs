using System;

namespace SimpleClassCreator
{
    public enum SourceTypeEnum
    {
        Query,
        TableName
    }

    public enum CodeType
    {
        CSharp = 0,
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
}
