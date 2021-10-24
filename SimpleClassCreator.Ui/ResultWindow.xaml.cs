using System.Windows;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(string title, string contents)
        {
            InitializeComponent();

            Title = title;

            TxtResult.Text = contents;
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxtResult.Text);
        }
    }
}
