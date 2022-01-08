using System.Collections.Generic;
using System.Linq;

namespace SimpleClassCreator.Lib.Models
{
    public class AssemblyInfo
    {
        public AssemblyInfo()
        {
            Classes = new List<ClassInfo>();
        }

        public string Name { get; set; }

        public List<ClassInfo> Classes { get; }

        public ClassInfo Add(string fullyQualifiedClassName)
        {
            var info = new ClassInfo { FullName = fullyQualifiedClassName };

            Classes.Add(info);

            return info;
        }

        public void Add(IEnumerable<string> fullyQualifiedClassNames)
        {
            var lst = fullyQualifiedClassNames.Select(x => new ClassInfo { FullName = x });

            Classes.AddRange(lst);
        }

        //public void Add(ClassInfo info)
        //{
        //    Classes.Add(info);
        //}
    }
}
