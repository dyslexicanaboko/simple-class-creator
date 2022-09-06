using SimpleClassCreator.Lib.Models.Meta;
using System.Collections.ObjectModel;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaClassViewModel 
        : ObservableObject, IMetaClass
    {
        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        //Type cannot be enforced by interface
        public ObservableCollection<MetaPropertyViewModel> Properties { get; set; }

        //View properties
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;

                OnPropertyChanged(nameof(IsChecked));
            }
        }
    }
}
