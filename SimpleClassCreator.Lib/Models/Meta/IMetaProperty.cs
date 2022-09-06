namespace SimpleClassCreator.Lib.Models.Meta
{
    public interface IMetaProperty
    {
        string Name { get; set; }
        string TypeName { get; set; }
        bool IsPrimitive { get; set; }
        bool IsEnum { get; set; }
        bool IsInterface { get; set; }
        bool IsSerializable { get; set; }
        bool IsCollection { get; set; }
    }
}