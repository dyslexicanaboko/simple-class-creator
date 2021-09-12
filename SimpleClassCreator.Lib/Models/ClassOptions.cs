using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassCreator.Lib.Models
{
    /// <summary>
    /// Generation options. Each property should be read as "Generate PropertyName".
    /// </summary>
    public class ClassOptions
    {
        public bool Entity { get; set; }
        
        public string EntityName { get; set; }

        public bool Model { get; set; }

        public string ModelName { get; set; }

        public bool Interface { get; set; }
    }
}
