using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Models.Meta
{
    public interface IMetaAssembly
    {
        string Name { get; set; }
        
        List<IMetaClass> Classes { get; }
    }
}