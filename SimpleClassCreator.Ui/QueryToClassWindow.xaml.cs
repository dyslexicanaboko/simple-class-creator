using System.Windows;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for QueryToClassWindow.xaml
    /// </summary>
    public partial class QueryToClassWindow
        : Window
    {
        public QueryToClassWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ctrlQueryToClass.CloseResultWindows();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            using (var obj = new AboutSimpleClassCreator())
            {
                obj.ShowDialog();
            }
        }
    }
}
