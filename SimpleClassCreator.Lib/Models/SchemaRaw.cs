using System.Data;

namespace SimpleClassCreator.Lib.Models
{
    public class SchemaRaw
    {
        public DataTable GenericSchema { get; set; }
        
        public DataTable SqlServerSchema { get; set; }
    }
}
