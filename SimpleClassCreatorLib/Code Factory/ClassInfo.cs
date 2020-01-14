using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Code_Factory
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
