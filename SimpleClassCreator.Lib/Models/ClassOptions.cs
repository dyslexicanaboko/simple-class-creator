namespace SimpleClassCreator.Lib.Models
{
    /// <summary>
    /// Generation options. Each property should be read as "Generate PropertyName".
    /// </summary>
    public class ClassOptions
    {
        public bool GenerateEntity { get; set; }
        
        public bool GenerateEntityIEquatable { get; set; }
        
        public bool GenerateEntityIComparable { get; set; }

        public bool GenerateEntityEqualityComparer { get; set; }

        public string EntityName { get; set; }

        public bool GenerateModel { get; set; }

        public string ModelName { get; set; }

        public bool GenerateInterface { get; set; }
    }
}
