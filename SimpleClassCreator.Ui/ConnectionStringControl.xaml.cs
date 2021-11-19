using SimpleClassCreator.Lib.DataAccess;
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

        public ConnectionManager VerifiedConnections { get; } = new ConnectionManager();

        public ConnectionManager.Connection CurrentConnection
        {
            get
            {
                if (CbConnectionString.SelectedIndex > -1)
                    return (ConnectionManager.Connection)CbConnectionString.SelectedItem;

                var obj = new ConnectionManager.Connection();
                obj.Verified = false;
                obj.ConnectionString = CbConnectionString.Text;

                return obj;
            }
        }

        public ConnectionStringControl()
        {
            InitializeComponent();

            CbConnectionString_Refresh();
        }

        public void Dependencies(
            IGeneralDatabaseQueries repository)
        {
            _generalRepo = repository;
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
                new ObservableCollection<ConnectionManager.Connection>(VerifiedConnections.Connections);
        }

        public bool TestConnectionString(bool showMessageOnFailureOnly = false)
        {
            var con = CurrentConnection;

            var obj = _generalRepo.TestConnectionString(con.ConnectionString);

            con.Verified = obj.Success;

            VerifiedConnections.UpdateConnection(con);

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
