namespace SimpleClassCreator.Lib.Models
{
    public class ClassProperty
    {
        public string Name { get; set; }
        
        public string TypeName { get; set; }
        
        public bool IsPrimitive { get; set; }
        
        public bool IsEnum { get; set; }
        
        public bool IsInterface { get; set; }

        public bool IsSerializable { get; set; }
        
        public bool IsCollection { get; set; }

        public override string ToString()
        {
            return Name + " " + TypeName;
        }
    }
}
