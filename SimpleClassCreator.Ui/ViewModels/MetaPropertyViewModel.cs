using SimpleClassCreator.Lib.Models.Meta;

namespace SimpleClassCreator.Ui.ViewModels
{
    public class MetaPropertyViewModel 
        : ObservableObject, IMetaProperty
    {
        public string Name { get; set; }
        
        public string TypeName { get; set; }
        
        public bool IsPrimitive { get; set; }
        
        public bool IsEnum { get; set; }
        
        public bool IsInterface { get; set; }

        public bool IsSerializable { get; set; }
        
        public bool IsCollection { get; set; }

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
