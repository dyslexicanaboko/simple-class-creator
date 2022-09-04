using SimpleClassCreator.Lib.Models.Meta;
using System.Collections.Generic;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaAssemblyViewModel : IMetaAssembly
    {
        public string Name { get; set; }

        //Type cannot be enforced by interface
        public List<MetaClassViewModel> Classes { get; set; }
    }
}
