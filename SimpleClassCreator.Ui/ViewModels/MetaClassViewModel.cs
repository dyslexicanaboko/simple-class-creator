using System.Collections.Generic;
using SimpleClassCreator.Lib.Models.Meta;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaClassViewModel : IMetaClass
    {
        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        //Type cannot be enforced by interface
        public List<MetaPropertyViewModel> Properties { get; set; }
    }
}
