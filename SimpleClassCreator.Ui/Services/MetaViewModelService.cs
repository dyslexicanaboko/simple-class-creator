using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Ui.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SimpleClassCreator.Ui.Services
{
    public class MetaViewModelService
    {
        public MetaAssemblyViewModel ToViewModel(MetaAssembly assembly)
        {
            var vmAsm = new MetaAssemblyViewModel();

            vmAsm.Name = assembly.Name;
            vmAsm.Classes = new List<MetaClassViewModel>(assembly.Classes.Count);

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
                        TypeName = x.TypeName
                    })
                    .ToList();

                //Create the class last
                var vmC = new MetaClassViewModel
                {
                    FullName = cls.FullName,
                    Name = cls.Name,
                    Namespace = cls.Namespace,
                    Properties = vmP
                };

                vmAsm.Classes.Add(vmC);
            }

            return vmAsm;
        }
    }
}
