using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Code_Factory
{
    public class PropertyInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsSerializable { get; set; }
        public bool IsCollection { get; set; }

        public override string ToString()
        {
            return Name + " " + TypeName;
        }
    }
}
