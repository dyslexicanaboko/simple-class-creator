using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models
{
    public class SchemaQuery
    {
        public string Query { get; set; }

        public bool IsSingleTableQuery { get; set; }

        public TableQuery Table { get; set; }

        public bool HasPrimaryKey { get; set; }

        public SchemaColumn PrimaryKey { get; set; }

        public IList<SchemaColumn> ColumnsAll { get; set; }
        
        public IList<SchemaColumn> ColumnsNoPk { get; set; }
    }
}
