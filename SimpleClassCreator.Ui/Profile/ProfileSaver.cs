using Newtonsoft.Json;
using System;
using System.IO;

namespace SimpleClassCreator.Ui.Profile
{
    public class ProfileSaver
    {
        private readonly object _fileLock = new object();

        private readonly string _profileFilePath;

        private ProfileManager _profileManager;

        public ProfileSaver()
        {
            _profileFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profile.json");
        }

        public ProfileManager Load()
        {
            lock (_fileLock)
            {
                var json = File.ReadAllText(_profileFilePath);

                _profileManager = JsonConvert.DeserializeObject<ProfileManager>(json);
                _profileManager.RegisterSaveDelegate(SaveHandler);

                return _profileManager;
            }
        }

        public void SaveHandler(object sender, EventArgs e) => Save();

        public void Save()
        {
            lock (_fileLock)
            {
                var json = JsonConvert.SerializeObject(_profileManager, Formatting.Indented);

                File.WriteAllText(_profileFilePath, json);
            }
        }
    }
}
