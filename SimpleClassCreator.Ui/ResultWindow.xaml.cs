using Microsoft.Win32;
using System.IO;
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

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = Title; // Default file name
            dlg.DefaultExt = ".cs"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            var result = dlg.ShowDialog().GetValueOrDefault();

            if (!result) return;

            File.WriteAllText(dlg.FileName, TxtResult.Text);
        }
    }
}
