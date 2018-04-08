using DevExpress.Mvvm;
using IDQ_Bot.Model.Classes.PlanetRomeo;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_DataModel.PlanetRomeo.Struct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel: ViewModelBase
    {
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
                LabelMinAge = string.Format("Minimal Age = {0} Maximal = {1}", MinAge, MaxAge);
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
                LabelMinAge = string.Format("Minimal Age = {0} Maximal = {1}", MinAge, MaxAge);
                RaisePropertiesChanged("MaxAge", "LabelMinAge");
            }
        }
        public string LabelMinAge { get; set; }

        public PRSearchSettings SelectedSearchSettings { get; set; }
        public ObservableCollection<PRSearchSettings> ListSearchSettings { get; set; }
    }
}
