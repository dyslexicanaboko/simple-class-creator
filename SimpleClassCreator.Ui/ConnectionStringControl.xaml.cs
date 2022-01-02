using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Ui.Profile;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleClassCreator.Lib.Models;
using B = SimpleClassCreator.Ui.UserControlExtensions;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for ConnectionStringControl.xaml
    /// </summary>
    public partial class ConnectionStringControl : UserControl
    {
        private IGeneralDatabaseQueries _generalRepo;

        public ConnectionStringManager UserConnectionStrings { get; private set; }

        public UserConnectionString CurrentConnection
        {
            get
            {
                if (CbConnectionString.SelectedIndex > -1)
                    return (UserConnectionString)CbConnectionString.SelectedItem;

                var obj = new UserConnectionString();
                obj.Verified = false;
                obj.ConnectionString = CbConnectionString.Text;

                return obj;
            }
        }

        public ConnectionStringControl()
        {
            InitializeComponent();
        }

        public void Dependencies(
            IProfileManager profileManager,
            IGeneralDatabaseQueries repository)
        {
            _generalRepo = repository;

            UserConnectionStrings = profileManager.ConnectionStringManager;

            CbConnectionString_Refresh();
        }

        private async void BtnConnectionStringTest_Click(object sender, RoutedEventArgs e)
        {
            await TestConnectionStringNonBlocking();
        }

        private async void CbConnectionString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                await TestConnectionStringNonBlocking();
        }

        private void CbConnectionString_Refresh()
        {
            CbConnectionString.ItemsSource =
                new ObservableCollection<UserConnectionString>(UserConnectionStrings.ConnectionStrings);
        }

        private async Task TestConnectionStringNonBlocking()
        {
            try
            {
                BtnConnectionStringTest.IsEnabled = false;
                PbConnectionTest.IsIndeterminate = true;

                var con = CurrentConnection;

                var result = await Task.Run(() => TestConnectionString(con));

                ShowResult(result);
            }
            finally
            {
                BtnConnectionStringTest.IsEnabled = true;
                PbConnectionTest.IsIndeterminate = false;
            }
        }

        private ConnectionResult TestConnectionString(UserConnectionString userConnectionString)
        {
            var obj = _generalRepo.TestConnectionString(userConnectionString.ConnectionString);

            userConnectionString.Verified = obj.Success;

            UserConnectionStrings.Update(userConnectionString);

            return obj;
        }

        private bool ShowResult(ConnectionResult result, bool showMessageOnFailureOnly = false)
        {
            CbConnectionString_Refresh();

            var showMessage = true;

            if (showMessageOnFailureOnly)
                showMessage = !result.Success;

            if (showMessage)
                B.ShowWarningMessage(result.Success ? "Connected Successfully" : "Connection Failed. Returned error: " + result.Message);

            return result.Success;
        }

        public bool TestConnectionString(bool showMessageOnFailureOnly = false)
        {
            var obj = TestConnectionString(CurrentConnection);

            return ShowResult(obj, showMessageOnFailureOnly);
        }
    }
}
