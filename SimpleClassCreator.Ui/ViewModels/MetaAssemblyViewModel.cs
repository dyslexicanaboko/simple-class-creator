using SimpleClassCreator.Lib.Models.Meta;
using System.Collections.Generic;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class AssemblyInfoModel : IMetaAssembly
    {
        public string Name { get; set; }

        public List<IMetaClass> Classes { get; }
    }
}
