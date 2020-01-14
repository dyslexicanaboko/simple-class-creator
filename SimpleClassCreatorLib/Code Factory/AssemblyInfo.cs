using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Code_Factory
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
