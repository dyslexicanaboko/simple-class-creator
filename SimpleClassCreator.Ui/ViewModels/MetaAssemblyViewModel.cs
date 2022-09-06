using SimpleClassCreator.Lib.Models.Meta;
using System.Collections.ObjectModel;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaAssemblyViewModel : IMetaAssembly
    {
        public string Name { get; set; }

        public ObservableCollection<MetaClassViewModel> Classes { get; set; }
    }
}
