namespace SimpleClassCreator.Lib.Models
{
    public class ClassParameters
    {
        public CodeType LanguageType { get; set; }
        public SourceTypeEnum SourceType { get; set; }
        public bool BuildOutClassProperties { get; set; }
        public bool ExcludeCollections { get; set; }
        public bool IncludeCloneMethod { get; set; }
        public bool IncludeIEquatableOfTMethods { get; set; }
        public bool IncludeFieldPrefix { get; set; }
        public bool IncludeNamespace { get; set; }
        public bool IncludeSerializeablePropertiesOnly { get; set; }
        public bool IncludeTranslateMethod { get; set; }
        public bool IncludeSerializableAttribute { get; set; }
        public bool ReplaceExisting { get; set; }
        public bool SaveAsFile { get; set; }
        public string ClassName { get; set; }
        public string ClassSource { get; set; }
        public string ConnectionString { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public string MemberPrefix { get; set; }
        public string Namespace { get; set; }
        public TableQuery TableQuery { get; set; } = new TableQuery();
    }
}