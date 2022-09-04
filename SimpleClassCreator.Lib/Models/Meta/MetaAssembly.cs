using System.Collections.Generic;
using System.Linq;

namespace SimpleClassCreator.Lib.Models.Meta
{
    public class MetaAssembly
    {
        public MetaAssembly()
        {
            Classes = new List<MetaClass>();
        }

        public string Name { get; set; }

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
