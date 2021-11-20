using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Ui.Profile;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void BtnConnectionStringTest_Click(object sender, RoutedEventArgs e)
        {
            TestConnectionString();
        }

        private void CbConnectionString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TestConnectionString();
        }

        private void CbConnectionString_Refresh()
        {
            CbConnectionString.ItemsSource =
                new ObservableCollection<UserConnectionString>(UserConnectionStrings.ConnectionStrings);
        }

        public bool TestConnectionString(bool showMessageOnFailureOnly = false)
        {
            var con = CurrentConnection;

            var obj = _generalRepo.TestConnectionString(con.ConnectionString);

            con.Verified = obj.Success;

            UserConnectionStrings.Update(con);

            CbConnectionString_Refresh();

            var showMessage = true;

            if (showMessageOnFailureOnly)
                showMessage = !obj.Success;

            if (showMessage)
                B.ShowWarningMessage(obj.Success ? "Connected Successfully" : "Connection Failed. Returned error: " + obj.Message);

            return obj.Success;
        }
    }
}
