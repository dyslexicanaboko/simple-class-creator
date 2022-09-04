﻿using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models.Meta
{
    public class MetaClass : IMetaClass
    {
        public MetaClass()
        {
            Properties = new List<IMetaProperty>();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<IMetaProperty> Properties { get; set; }
    }
}
