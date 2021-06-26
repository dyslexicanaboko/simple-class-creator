using System.Collections.Generic;

namespace SimpleClassCreator.Models
{
    public class ClassInfo
    {
        public ClassInfo()
        {
            Properties = new List<PropertyInfo>();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<PropertyInfo> Properties { get; set; }
    }
}
