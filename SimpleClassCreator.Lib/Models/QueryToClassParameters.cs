namespace SimpleClassCreator.Lib.Models
{
    public class QueryToClassParameters
    {
        /// <summary>
        /// Connection string to use for executing the provided SQL
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// This may be phased out, offers the option of C# versus VB.Net but this may not matter anymore because this project is moving towards
        /// template based generation. If someone wants to keep using inferior VB.Net they can put in the work to make a shitty template for it.
        /// </summary>
        public CodeType LanguageType { get; set; }
        
        /// <summary>
        /// Determines if the Source SQL Text contains just a table name or a full SQL Query
        /// </summary>
        public SourceSqlType SourceSqlType { get; set; }

        /// <summary>
        /// Can be just the name of a table or a full SQL query
        /// </summary>
        public string SourceSqlText { get; set; }

        /// <summary>
        /// Structured object that formats the incoming Source SQL Text
        /// </summary>
        public TableQuery TableQuery { get; set; } = new TableQuery();

        /// <summary>
        /// File name only
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Path where file will be saved
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Save generated output as a file
        /// </summary>
        public bool SaveAsFile { get; set; }

        /// <summary>
        /// Overwrite existing files that may have the same name
        /// </summary>
        public bool OverwriteExistingFiles { get; set; }

        /// <summary>
        /// Namespace used for all classes that are generated
        /// </summary>
        public string Namespace { get; set; }

        public ClassOptions ClassOptions { get; set; } = new ClassOptions();

        public ClassServices ClassServices { get; set; } = ClassServices.None;
        
        public ClassRepositories ClassRepositories { get; set; } = ClassRepositories.None;

        public bool HasElections => 
            ClassOptions.GenerateEntity || 
            ClassOptions.GenerateModel || 
            ClassOptions.GenerateInterface;
    }
}