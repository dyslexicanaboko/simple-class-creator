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
            get => TxtValue.Text;
            set => TxtValue.Text = value;
        }

        public string DefaultText { get; set; }

        private void BtnDefault_Click(object sender, RoutedEventArgs e)
        {
            Text = DefaultText;
        }
    }
}
