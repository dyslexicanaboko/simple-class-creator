using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Ui.ViewModels;
using System.ComponentModel;

namespace SimpleClassCreator.Ui.Services
{
    public interface IMetaViewModelService
    {
        MetaAssemblyViewModel ToViewModel(MetaAssembly assembly, PropertyChangedEventHandler metaClassCbToggled);
    }
}