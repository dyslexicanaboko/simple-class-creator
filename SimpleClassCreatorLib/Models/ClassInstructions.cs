using SimpleClassCreator.Services.CodeFactory;
using System.Collections.Generic;

namespace SimpleClassCreator.Models
{
    public class ClassInstructions
    {
        public List<string> Namespaces { get; set; } = new List<string>();
        
        public List<string> Attributes { get; set; } = new List<string>();

        public List<ClassMemberStrings> Properties { get; set; } = new List<ClassMemberStrings>();
    }
}
