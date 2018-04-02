using DevExpress.Mvvm;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    static class PlacesHelper
    {
        public static List<string> GetListContinents(this List<Continent> continents)
        {
            var temp = new List<string>();
            foreach (var item in continents)
            {
                temp.Add(item.Name);
            }
            return temp;
        }
        public static List<string> GetListCountries(this List<Continent> continents, string continent)
        {
            var temp = new List<string>();
            foreach (var item in continents.Find((x) => x.Name == continent).Countryes)
            {
                temp.Add(item.Name);
            }
            return temp;
        }
        public static List<string> GetListArea(this List<Continent> continents, string continent, string country)
        {
            var temp = new List<string>();
            if (country != null)
            {
                var tempcountries = continents.Find((x) => x.Name == continent).Countryes;
                foreach (var item in tempcountries.Find((y) => y.Name == country).Areaes)
                {
                    temp.Add(item.Name);
                }
            }
            return temp;
        }
        public static PRSearchSettings ToID(this PRSearchSettings search, List<Continent> continents)
        {
            var contid = continents.Find((x) => x.Name == search.Continent).Id;
            var counid = continents.Find((x) => x.Name == search.Continent).Countryes.Find((y) => y.Name == search.Country).Id;
            var areaid = continents.Find((x) => x.Name == search.Continent).Countryes.Find((y) => y.Name == search.Country).Areaes.Find((z) => z.Name == search.Area).Id;
            return new PRSearchSettings(contid, counid, areaid, search.MinAge, search.MaxAge);
        }
    }

    class SSSettings
    {
        public PRSearchSettings SelectedSearchSettings { get; set; }
        public ObservableCollection<PRSearchSettings> ListSearchSettings { get; set; }
    }

    partial class PRViewModel : ViewModelBase
    {
        private string _selectedContinent;
        private string _selectedCountry;
        private string _selectedArea;

        private int _minAge;
        private int _maxAge;

        public ObservableCollection<string> ItemsContinent { get; set; }
        public ObservableCollection<string> ItemsCountry { get; set; }
        public ObservableCollection<string> ItemsAreas { get; set; }

        public string SelectedContinent
        {
            get => _selectedContinent;
            set
            {
                _selectedContinent = value;
                if (ItemsCountry == null)
                {
                    ItemsCountry = new ObservableCollection<string>(places.GetListCountries(value));
                    SelectedCountry = ItemsCountry.First();

                }
                else
                {
                    ItemsCountry.Clear();
                    foreach (var item in places.GetListCountries(value))
                    {
                        ItemsCountry.Add(item);
                    }

                    SelectedCountry = ItemsCountry.First();
                }
                RaisePropertyChanged("SelectedContinent");
            }
        }
        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (value != null)
                {
                    _selectedCountry = value;
                    if (ItemsAreas == null)
                    {
                        ItemsAreas = new ObservableCollection<string>(places.GetListArea(SelectedContinent, _selectedCountry));
                        SelectedArea = ItemsAreas.First();
                    }
                    else
                    {
                        ItemsAreas.Clear();
                        foreach (var item in places.GetListArea(SelectedContinent, _selectedCountry))
                        {
                            ItemsAreas.Add(item);
                        }
                        if (ItemsAreas.Count > 0)
                            SelectedArea = ItemsAreas.First();
                    }
                    RaisePropertiesChanged("SelectedArea", "SelectedArea");
                }
                else
                {
                    if (ItemsCountry.Count > 0)
                        _selectedCountry = ItemsCountry.First();
                    if (ItemsAreas == null)
                    {
                        ItemsAreas = new ObservableCollection<string>(places.GetListArea(SelectedContinent, _selectedCountry));
                        SelectedArea = ItemsAreas.First();
                    }
                    else
                    {
                        ItemsAreas.Clear();
                        foreach (var item in places.GetListArea(SelectedContinent, value))
                        {
                            ItemsAreas.Add(item);
                        }
                        if (ItemsAreas.Count > 0)
                            SelectedArea = ItemsAreas.First();
                    }
                    RaisePropertiesChanged("SelectedArea", "SelectedArea");
                }
            }
        }
        public string SelectedArea { get => _selectedArea; set => _selectedArea = value; }

        public int MinAge
        {
            get => _minAge;
            set
            {
                _minAge = value;
                if (MinAge > MaxAge) MaxAge = MinAge;
                LabelMinAge = string.Format("Minimal Age = {0}", MinAge.ToString());
                RaisePropertiesChanged("MinAge", "LabelMinAge");
            }
        }
        public int MaxAge
        {
            get => _maxAge;
            set
            {
                _maxAge = value;
                if (MaxAge < MinAge) MinAge = MaxAge;
                LabelMaxAge = string.Format("Maximal Age = {0}", MaxAge.ToString());
                RaisePropertiesChanged("MaxAge", "LabelMaxAge");
            }
        }
        public string LabelMinAge { get; set; }
        public string LabelMaxAge { get; set; }

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

        //************************************************************************************************************************************

        private const string SSPATH = "Settings/PlanetRomeo/SearchSettings.json";
        private const string PLASES = "Settings/PlanetRomeo/Places.json";
        private SSSettings sssettings;
        private List<Continent> places = JSONSerializer.Deserialize<List<Continent>>(FileManager.ReadTextFromFile(PLASES));

        public PRSearchSettings SelectedSearchSettings { get; set; }
        public ObservableCollection<PRSearchSettings> ListSearchSettings { get; set; }


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
    }
}
