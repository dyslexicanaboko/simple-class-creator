using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models
{
    public class ClassInfo
    {
        public ClassInfo()
        {
            Properties = new List<ClassProperty>();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<ClassProperty> Properties { get; set; }
    }
}
