using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassCreator.Lib.Models
{
    public class CompilerResult
    {
        public bool CompiledSuccessfully { get; set; }

        public string ClassName { get; set; }

        public object Instance { get; set; }

        public Type Type { get; set; }

        public List<string> Errors { get; set; }
    }
}
