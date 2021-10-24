namespace SimpleClassCreator.Lib.Models
{
    public class TableQuery
    {
        public string LinkedServer { get; set; }
        
        public string Database { get; set; }
        
        public string Schema { get; set; }

        /// <summary>Qualified version of the table name.</summary>
        public string Table { get; set; }

        /// <summary>Unqualified version of the table name.</summary>
        public string TableUnqualified { get; set; }
    }
}
