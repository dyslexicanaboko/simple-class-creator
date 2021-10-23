namespace SimpleClassCreator.Lib.Models
{
    public class DtoMakerParameters
    {
        public bool ExcludeCollections { get; set; }
        public bool IncludeCloneMethod { get; set; }
        public bool IncludeIEquatableOfTMethods { get; set; }
        public bool IncludeTranslateMethod { get; set; }
    }
}