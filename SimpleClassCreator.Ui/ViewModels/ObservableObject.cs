using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleClassCreator.Ui.ViewModels
{
    //Excellent explanation of how to handle observable collections
    //https://www.youtube.com/watch?v=gOf2FZ6dkbU&ab_channel=Payload
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
