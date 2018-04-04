using IDQ_Bot.Model.Classes.PlanetRomeo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel
    {
        private const string PRMSETPATH = "Settings/PlanetRomeo/MessagesSettings.json";
        private MessagesSettings msettings;
        private string _countMessage;
        private List<string> _listTypeMessage;
        private string _selectedTypeMessage;
    }
}
