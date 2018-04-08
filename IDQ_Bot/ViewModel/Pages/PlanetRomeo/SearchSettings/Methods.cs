using DevExpress.Mvvm;
using IDQ_Bot.Model.Classes.PlanetRomeo;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Class;
using IDQ_DataModel.PlanetRomeo.Struct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel : ViewModelBase
    {
        private void InitSearchSettings()
        {
            if (FileManager.IsExist(SSPATH))
            {
                sssettings = JSONSerializer.Deserialize<SSSettings>(FileManager.ReadTextFromFile(SSPATH));
                ListSearchSettings = sssettings.ListSearchSettings;
                SelectedSearchSettings = sssettings.SelectedSearchSettings;
            }
            else
            {
                ListSearchSettings = new ObservableCollection<PRSearchSettings> { new PRSearchSettings("Europe", "Germany", "Berlin", "40", "50") };
                SelectedSearchSettings = ListSearchSettings.Last();
                sssettings = new SSSettings();
                sssettings.ListSearchSettings = ListSearchSettings;
                sssettings.SelectedSearchSettings = SelectedSearchSettings;
                sssettings.SerializeToFile(SSPATH);
            }

            ItemsContinent = new ObservableCollection<string>(places.GetListContinents());
            SelectedContinent = ItemsContinent.First();
            MinAge = 18;
            MaxAge = 18;
        }

        public DelegateCommand AddSearchSettings
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var tempSS = new PRSearchSettings(SelectedContinent, SelectedCountry, SelectedArea,
                        MinAge.ToString(), MaxAge.ToString());

                    if (!ListSearchSettings.Contains(tempSS))
                    {
                        ListSearchSettings.Add(tempSS);
                        SelectedSearchSettings = ListSearchSettings.Last();

                        sssettings.SerializeToFile(SSPATH);
                        RaisePropertiesChanged("ListSearchSettings", "SelectedSearchSettings");
                    }
                });
            }
        }
        public DelegateCommand DeleteSearchSettings
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (ListSearchSettings.Contains(SelectedSearchSettings))
                    {
                        ListSearchSettings.Remove(SelectedSearchSettings);
                        SelectedSearchSettings = ListSearchSettings.Count > 0 ? ListSearchSettings.Last() : new PRSearchSettings();

                        sssettings.SerializeToFile(SSPATH);
                        RaisePropertiesChanged("ListSearchSettings", "SelectedSearchSettings");
                    }
                }, () => ListSearchSettings.Count > 0);
            }
        }
    }
}
