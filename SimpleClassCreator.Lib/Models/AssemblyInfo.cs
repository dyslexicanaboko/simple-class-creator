using System.Collections.Generic;
using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Models
{
    public class AssemblyInfo
    {
        public AssemblyInfo()
        {
            Classes = new List<ClassInfo>();
        }

        public string Name { get; set; }

        public List<ClassInfo> Classes { get; set; }

        public ClassInfo AddClass(string fullyQualifiedClassName)
        {
            ClassInfo info = new ClassInfo { FullName = fullyQualifiedClassName };

            Classes.Add(info);

            return info;
        }

        public void AddClass(ClassInfo info)
        {
            Classes.Add(info);
        }
    }
}
