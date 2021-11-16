using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using B = SimpleClassCreator.Ui.UserControlExtensions;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Segregating the Connection String code into its own file so I can decouple it from this
    /// control in the future. It has becomes its own beast and shouldn't be together with this
    /// control anymore.
    /// </summary>
    public partial class QueryToClassControl
    {
        private ConnectionManager VerifiedConnections { get; }

        private ConnectionManager.Connection CurrentConnection
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

        private void CbConnectionString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TestConnectionString();
        }

        private bool TestConnectionString(bool showMessageOnFailureOnly = false)
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

        private void BtnConnectionStringTest_Click(object sender, RoutedEventArgs e)
        {
            TestConnectionString();
        }

        private void CbConnectionString_Refresh()
        {
            CbConnectionString.ItemsSource =
                new ObservableCollection<ConnectionManager.Connection>(VerifiedConnections.Connections);
        }
    }
}
