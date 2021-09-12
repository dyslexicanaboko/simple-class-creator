using System.Windows;
using System.Windows.Controls;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for TextBoxWithDefaultControl.xaml
    /// </summary>
    public partial class TextBoxWithDefaultControl : UserControl
    {
        public TextBoxWithDefaultControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }
        
        public string DefaultText { get; set; }

        //Default behavior of the Default Button is to just set Text to whatever the DefaultText string is set to
        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            Text = DefaultText;
        }
    }
}
