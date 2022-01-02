namespace SimpleClassCreator.Lib.Models
{
    public class QueryToMockDataParameters
    {
        /// <summary>
        /// Connection string to use for executing the provided SQL
        /// </summary>
        public string ConnectionString { get; set; }
        
        /// <summary>
        /// Determines if the Source SQL Text contains just a table name or a full SQL Query
        /// </summary>
        public SourceSqlType SourceSqlType { get; set; }

        /// <summary>
        /// Can be just the name of a table or a full SQL query
        /// </summary>
        public string SourceSqlText { get; set; }

        /// <summary>
        /// Structured object that formats the incoming Source SQL Text
        /// </summary>
        public TableQuery TableQuery { get; set; } = new TableQuery();
        
        public string ClassEntityName { get; set; }
    }
}