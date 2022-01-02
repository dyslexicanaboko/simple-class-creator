using System;
using System.Collections.Generic;

namespace SimpleClassCreator.Ui.Profile
{
    public class ConnectionStringManager
    {
        public delegate void SaveHandler(object sender, EventArgs e);
        
        public event SaveHandler Save;

        public int MaxConnectionStrings { get; set; }

        public List<UserConnectionString> ConnectionStrings { get; set; }
        
        public void Update(UserConnectionString target)
        {
            var inList = ConnectionStrings.Find(x => x.ConnectionString == target.ConnectionString);

            if (inList == null && target.Verified)
            {
                //If the maximum amount of connections has been reached
                if (ConnectionStrings.Count == MaxConnectionStrings)
                    ConnectionStrings.RemoveAt(ConnectionStrings.Count - 1); //Then remove the last item

                //Add the new connection to the top of the list
                ConnectionStrings.Insert(0, target);

                RaiseSaveEvent();
            }
            else if (!target.Verified)
            {
                ConnectionStrings.Remove(inList);

                RaiseSaveEvent();
            }
        }

        private void RaiseSaveEvent()
        {
            Save?.Invoke(this, new EventArgs());
        }
    }
}
