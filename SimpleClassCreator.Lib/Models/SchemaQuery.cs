using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models
{
    public class SchemaQuery
    {
        public string Query { get; set; }

        /// <summary>
        /// Denotes whether or not the original query is for a solitary table or if the query
        /// involves more than one table.
        /// </summary>
        public bool IsSolitaryTableQuery { get; set; }

        public TableQuery TableQuery { get; set; }

        public bool HasPrimaryKey { get; set; }

        public SchemaColumn PrimaryKey { get; set; }

        public IList<SchemaColumn> ColumnsAll { get; set; }
        
        public IList<SchemaColumn> ColumnsNoPk { get; set; }
    }
}
