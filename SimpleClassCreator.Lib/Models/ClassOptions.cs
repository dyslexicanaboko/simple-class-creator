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
        public bool GenerateEntity { get; set; }
        
        public string EntityName { get; set; }

        public bool GenerateModel { get; set; }

        public string ModelName { get; set; }

        public bool GenerateInterface { get; set; }
    }
}
