using System.Collections.Generic;
using SimpleClassCreator.Lib.Services.CodeFactory;

namespace SimpleClassCreator.Lib.Models
{
    public class DtoInstructions
    {
        //Right now I am going to stop at top level implementation, no automatic nested implementations.
        //None of what is produced has to compile as far as I am concerned.
        //public bool ExcludeCollections { get; set; } //Might still need this... not sure yet

        public string ClassName { get; set; }

        public bool MethodEntityToDto { get; set; }

        public bool MethodDtoToEntity { get; set; }

        public bool ImplementIEquatableOfTInterface { get; set; }

        public bool ExtractInterface { get; set; }

        public IList<ClassMemberStrings> Properties { get; set; } = new List<ClassMemberStrings>();
    }
}