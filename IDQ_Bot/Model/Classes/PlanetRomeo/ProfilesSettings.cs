using IDQ_Bot.Model.Struct;
using System.Collections.ObjectModel;

namespace IDQ_Bot.Model.Classes.PlanetRomeo
{
    class ProfilesSettings
    {
        public Profile SavedSelectedProfile { get; set; }
        public ObservableCollection<Profile> List { get; set; }

        public ProfilesSettings()
        {
            SavedSelectedProfile = new Profile();
            List = new ObservableCollection<Profile>();
        }
    }
}
