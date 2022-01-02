using System.Windows;
using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Services;
using SimpleClassCreator.Ui.Profile;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
        : Window
    {
        public MainWindow(INameFormatService classService,
            IQueryToClassService queryToClassService,
            IGeneralDatabaseQueries repository,
            IProfileManager profileManager)
        {
            InitializeComponent();

            //I am probably doing this wrong, but I don't care right now. I will have to circle back and try to do it right later.
            //The MVVM model seems like a lot of extra unnecessary work.
            CtrlQueryToClass.Dependencies(
                classService, 
                queryToClassService, 
                repository, 
                profileManager);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CtrlQueryToClass.CloseResultWindows();
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
