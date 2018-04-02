using DevExpress.Mvvm;
using IDQ_Core_0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace IDQ_Bot.ViewModel
{
    class MainViewModel: ViewModelBase
    {
        private Page planetRomeo;
        
        public Page PlanetRomeo { get => planetRomeo; set => planetRomeo = value; }

        public MainViewModel()
        {
            FileManager.CreateDirectory("Data/PlanetRomeo");
            FileManager.CreateDirectory("Settings/PlanetRomeo");
            PlanetRomeo = new IDQ_Bot.View.Pages.PlanetRomeo();
        }
    }
}
