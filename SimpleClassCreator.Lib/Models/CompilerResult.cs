using System.Collections.Generic;
using System.Reflection;

namespace SimpleClassCreator.Lib.Models
{
    public class CompilerResult
    {
        public bool CompiledSuccessfully { get; set; }

        public string AssemblyPath { get; set; }

        public Assembly VirtualAssembly { get; set; }

        public List<string> Errors { get; set; }
    }
}
