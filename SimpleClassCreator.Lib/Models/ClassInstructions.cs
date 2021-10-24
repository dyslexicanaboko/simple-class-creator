using SimpleClassCreator.Lib.Services.CodeFactory;
using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models
{
    public class ClassInstructions
    {
        public TableQuery TableQuery { get; set; }

        /// <summary>Name of the main source entity</summary>
        public string EntityName { get; set; }
        
        /// <summary>Name of the source entity as an Entity class</summary>
        public string ClassEntityName { get; set; }

        /// <summary>Name of the source entity as a Model class</summary>
        public string ClassModelName { get; internal set; }
        
        /// <summary>Namespace used for all classes. It's just a container for the code and not intended for use.</summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Single interface name for now. May change it to be a list in the future,
        /// but I can't think of a reason as to why that would be warranted right now.
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>Should class be a partial class</summary>
        public bool IsPartial { get; set; }

        /// <summary>Namespaces that the class being generated should be using (importing).</summary>
        public IList<string> Namespaces { get; set; } = new List<string>();
        
        /// <summary>Class attributes</summary>
        public IList<string> ClassAttributes { get; set; } = new List<string>();

        /// <summary>
        /// Properties of the source entity.
        /// </summary>
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
                EntityName = EntityName,
                ClassEntityName = ClassEntityName,
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
