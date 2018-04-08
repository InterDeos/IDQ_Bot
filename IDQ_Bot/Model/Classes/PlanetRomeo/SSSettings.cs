using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_DataModel.PlanetRomeo.Struct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Classes.PlanetRomeo
{
    class SSSettings
    {
        public PRSearchSettings SelectedSearchSettings { get; set; }
        public ObservableCollection<PRSearchSettings> ListSearchSettings { get; set; }
    }
}
