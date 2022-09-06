//using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models.Meta
{
    public interface IMetaClass
    {
        string FullName { get; set; }
        
        string Name { get; set; }
        
        string Namespace { get; set; }
        
        //List<IMetaProperty> Properties { get; set; }
    }
}