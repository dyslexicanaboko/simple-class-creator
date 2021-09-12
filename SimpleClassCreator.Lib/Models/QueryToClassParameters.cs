namespace SimpleClassCreator.Lib.Models
{
    public class QueryToClassParameters
    {
        public CodeType LanguageType { get; set; }
        public SourceTypeEnum SourceType { get; set; }
        public bool ReplaceExisting { get; set; }
        public bool SaveAsFile { get; set; }
        public string ClassName { get; set; }
        public string ClassSource { get; set; }
        public string ConnectionString { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public string Namespace { get; set; }
        public TableQuery TableQuery { get; set; } = new TableQuery();
    }
}