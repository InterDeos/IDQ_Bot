using DevExpress.Mvvm;
using IDQ_Bot.Model.Struct;
using IDQ_Core_0.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
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

    partial class PRViewModel : ViewModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }

        public DelegateCommand AddProfile
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var tempProfile = new Profile(Login, Password, Language);
                    if (!ProfileList.Contains(tempProfile))
                    {
                        ProfileList.Add(tempProfile);
                        SelectedProfile = ProfileList.Last();
                        Login = "";
                        Password = "";
                        Language = "";

                        profilesSettings.SerializeToFile(PSPATH);
                        RaisePropertiesChanged("SelectedProfile", "ProfileList",
                            "Login", "Password", "Language");
                    }
                });
            }
        }
        public DelegateCommand DeleteProfile
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (ProfileList.Contains(SelectedProfile))
                    {
                        ProfileList.Remove(SelectedProfile);
                        SelectedProfile = ProfileList.Count > 0 ? ProfileList.Last() : new Profile();

                        profilesSettings.SerializeToFile(PSPATH);
                        RaisePropertiesChanged("SelectedProfile", "ProfileList");
                    }
                },()=> ProfileList.Count > 0);
            }
        }

        //****************************************************************************************************************

        private const string PSPATH = "Settings/PlanetRomeo/ProfilesSettings.json";
        private ProfilesSettings profilesSettings;

        public Profile SelectedProfile { get; set; }
        public ObservableCollection<Profile> ProfileList { get; set; }

        private void InitProfileSettings()
        {
            if (FileManager.IsExist(PSPATH))
            {
                profilesSettings = JSONSerializer.Deserialize<ProfilesSettings>(FileManager.ReadTextFromFile(PSPATH));
                ProfileList = profilesSettings.List;
                SelectedProfile = profilesSettings.SavedSelectedProfile;
            }
            else
            {
                ProfileList = new ObservableCollection<Profile>
                {
                    new Profile("sarumn89", "Hugarden2442", "Deuch")
                };
                SelectedProfile = ProfileList.Last();

                profilesSettings = new ProfilesSettings();
                profilesSettings.List = ProfileList;
                profilesSettings.SavedSelectedProfile = SelectedProfile;
                profilesSettings.SerializeToFile(PSPATH);
            }
        }
    }
}
