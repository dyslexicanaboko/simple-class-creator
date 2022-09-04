using System.Collections.Generic;
using System.Linq;

namespace SimpleClassCreator.Lib.Models.Meta
{
    //NOTE: I want the interface to enforce a list of interface, but its not going to work. Not sure how to deal with this right now.
    public class MetaAssembly : IMetaAssembly
    {
        public MetaAssembly()
        {
            Classes = new List<MetaClass>();
        }

        public string Name { get; set; }

        //Type cannot be enforced by interface
        public List<MetaClass> Classes { get; }

        public MetaClass Add(string fullyQualifiedClassName)
        {
            var info = new MetaClass { FullName = fullyQualifiedClassName };

            Classes.Add(info);

            return info;
        }

        public void Add(IEnumerable<string> fullyQualifiedClassNames)
        {
            var lst = fullyQualifiedClassNames.Select(x => new MetaClass { FullName = x });

            Classes.AddRange(lst);
        }
    }
}
