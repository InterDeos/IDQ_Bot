using IDQ_Bot.Model.Classes.PlanetRomeo;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel
    {
        private const string SSPATH = "Settings/PlanetRomeo/SearchSettings.json";
        private const string PLASES = "Settings/PlanetRomeo/Places.json";
        private SSSettings sssettings;
        private List<Continent> places = JSONSerializer.Deserialize<List<Continent>>(FileManager.ReadTextFromFile(PLASES));

        private string _selectedContinent;
        private string _selectedCountry;
        private string _selectedArea;

        private int _minAge;
        private int _maxAge;
    }
}
