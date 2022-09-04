using System.Collections.Generic;
using SimpleClassCreator.Lib.Models.Meta;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaClassViewModel : IMetaClass
    {
        public MetaClassViewModel()
        {
            Properties = new List<IMetaProperty>();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<IMetaProperty> Properties { get; set; }
    }
}
