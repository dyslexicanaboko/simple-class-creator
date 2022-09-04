using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models.Meta
{
    public class MetaClass
    {
        public MetaClass()
        {
            Properties = new List<MetaProperty>();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<MetaProperty> Properties { get; set; }
    }
}
