using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models
{
    public class CompilerResult
    {
        public bool CompiledSuccessfully { get; set; }

        public string AssemblyPath { get; set; }

        public List<string> Errors { get; set; }
    }
}
