using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models.Meta
{
    public class MetaClass : IMetaClass
    {
        public MetaClass()
        {
            Properties = new List<MetaProperty>();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        //Type cannot be enforced by interface
        public List<MetaProperty> Properties { get; set; }
    }
}
