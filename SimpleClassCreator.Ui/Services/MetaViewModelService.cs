using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Ui.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SimpleClassCreator.Ui.Services
{
    public class MetaViewModelService : IMetaViewModelService
    {
        public MetaAssemblyViewModel ToViewModel(MetaAssembly assembly, PropertyChangedEventHandler metaClassCbToggled)
        {
            var vmAsm = new MetaAssemblyViewModel();

            vmAsm.Name = assembly.Name;
            vmAsm.Classes = new ObservableCollection<MetaClassViewModel>();

            foreach (var cls in assembly.Classes)
            {
                //Get the properties first
                var vmP = cls.Properties
                    .Select(x => new MetaPropertyViewModel
                    {
                        IsInterface = x.IsInterface,
                        IsCollection = x.IsCollection,
                        IsPrimitive = x.IsPrimitive,
                        Name = x.Name,
                        IsEnum = x.IsEnum,
                        IsSerializable = x.IsSerializable,
                        TypeName = x.TypeName,
                        IsChecked = true
                    })
                    .ToList();

                //Create the class last
                var vmC = new MetaClassViewModel
                {
                    FullName = cls.FullName,
                    Name = cls.Name,
                    Namespace = cls.Namespace,
                    IsChecked = true,
                    Properties = new ObservableCollection<MetaPropertyViewModel>(vmP)
                };

                //Register event after properties are initialize to avoid triggering event accidentally
                vmC.PropertyChanged += metaClassCbToggled;

                vmAsm.Classes.Add(vmC);
            }

            return vmAsm;
        }
    }
}
