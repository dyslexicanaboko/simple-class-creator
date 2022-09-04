using SimpleClassCreator.Lib.Models.Meta;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaClassViewModel 
        : IMetaClass, INotifyPropertyChanged
    {
        public string FullName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        //Type cannot be enforced by interface
        public ObservableCollection<MetaPropertyViewModel> Properties { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //View properties
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }
    }
}
