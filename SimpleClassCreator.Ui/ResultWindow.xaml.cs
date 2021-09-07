using System.Windows;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(string resultText)
        {
            InitializeComponent();

            txtResult.Text = resultText;
        }
    }
}
