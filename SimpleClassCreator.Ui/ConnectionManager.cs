using System.Collections.Generic;

namespace SimpleClassCreator.Ui
{
    public class ConnectionManager
    {
        private const int MaxConnections = 10;

        public List<Connection> Connections { get; }
        
        public ConnectionManager()
        {
            Connections = LoadConnections();
        }

        private List<Connection> LoadConnections()
        {
            List<Connection> lst = new List<Connection>();

            string[] arr = Properties.Settings.Default.ConnectionsCSV.Split('|');

            foreach (string s in arr)
                lst.Add(new Connection { ConnectionString = s, Verified = true });

            return lst;
        }

        public void UpdateConnection(Connection target)
        {
            var inList = Connections.Find(x => x.ConnectionString == target.ConnectionString);

            if (inList == null && target.Verified)
            {
                //If the maximum amount of connections has been reached
                if (Connections.Count == MaxConnections)
                    Connections.RemoveAt(Connections.Count - 1); //Then remove the last item

                //Add the new connection to the top of the list
                Connections.Insert(0, target);

                SaveConnections();
            }
            else if (!target.Verified)
            {
                Connections.Remove(inList);

                SaveConnections();
            }
        }

        private void SaveConnections()
        {
            Properties.Settings.Default.ConnectionsCSV = string.Join("|", Connections);
            Properties.Settings.Default.Save();
        }

        public class Connection
        {
            public Connection()
            {
                ConnectionString = null;
                
                Verified = false;
            }

            public string ConnectionString { get; set; }

            public bool Verified { get; set; }

            public override string ToString()
            {
                return ConnectionString;
            }
        }
    }
}
