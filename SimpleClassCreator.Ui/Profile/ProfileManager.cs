using System;

namespace SimpleClassCreator.Ui.Profile
{
    public class ProfileManager
        : IProfileManager
    {
        public delegate void SaveHandler(object sender, EventArgs e);

        private event SaveHandler Save;

        public ConnectionStringManager ConnectionStringManager { get; set; }

        public void RegisterSaveDelegate(SaveHandler saveHandler)
        {
            Save += saveHandler;

            ConnectionStringManager.Save += RaiseSaveEvent;
        }

        private void RaiseSaveEvent(object sender, EventArgs e)
        {
            Save?.Invoke(this, new EventArgs());
        }
    }
}
