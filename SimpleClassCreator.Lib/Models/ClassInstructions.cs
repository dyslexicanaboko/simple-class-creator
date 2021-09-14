using SimpleClassCreator.Lib.Services.CodeFactory;
using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models
{
    public class ClassInstructions
    {
        public string ClassName { get; set; }
        
        public string Namespace { get; set; }
        
        /// <summary>
        /// Single interface name for now. May change it to be a list in the future,
        /// but I can't think of a reason as to why that would be warranted right now.
        /// </summary>
        public string InterfaceName { get; set; }
        
        public bool IsPartial { get; set; }

        public IList<string> Namespaces { get; set; } = new List<string>();
        
        public IList<string> ClassAttributes { get; set; } = new List<string>();

        public IList<ClassMemberStrings> Properties { get; set; } = new List<ClassMemberStrings>();

        public void AddNamespace(string nameSpace)
        {
            if (Namespaces.Contains(nameSpace)) return;

            Namespaces.Add(nameSpace);
        }

        public ClassInstructions Clone()
        {
            var c = new ClassInstructions
            {
                ClassName = ClassName,
                Namespace = Namespace,
                InterfaceName = InterfaceName
            };

            c.ClassAttributes = new List<string>(ClassAttributes);
            c.Namespaces = new List<string>(Namespaces);

            foreach (var p in Properties)
            {
                c.Properties.Add(p.Clone());
            }

            return c;
        }
    }
}
