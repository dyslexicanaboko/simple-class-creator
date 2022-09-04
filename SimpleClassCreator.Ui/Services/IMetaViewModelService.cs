using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Ui.ViewModels;

namespace SimpleClassCreator.Ui.Services
{
    public interface IMetaViewModelService
    {
        MetaAssemblyViewModel ToViewModel(MetaAssembly assembly);
    }
}