using SimpleClassCreator.Lib.Models.Meta;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaPropertyViewModel : IMetaProperty
    {
        public string Name { get; set; }
        
        public string TypeName { get; set; }
        
        public bool IsPrimitive { get; set; }
        
        public bool IsEnum { get; set; }
        
        public bool IsInterface { get; set; }

        public bool IsSerializable { get; set; }
        
        public bool IsCollection { get; set; }
    }
}
