using SimpleClassCreator.Services.CodeFactory;
using System.Collections.Generic;

namespace SimpleClassCreator.Models
{
    public class ClassInstructions
    {
        public string ClassName { get; set; }
        
        public string Namespace { get; set; }

        public IList<string> Namespaces { get; set; } = new List<string>();
        
        public IList<string> ClassAttributes { get; set; } = new List<string>();

        public IList<ClassMemberStrings> Properties { get; set; } = new List<ClassMemberStrings>();

        public void AddNamespace(string nameSpace)
        {
            if (Namespaces.Contains(nameSpace)) return;

            Namespaces.Add(nameSpace);
        }
    }
}
