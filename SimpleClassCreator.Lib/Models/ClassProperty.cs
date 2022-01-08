namespace SimpleClassCreator.Lib.Models
{
    public class ClassProperty
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
