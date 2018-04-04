using DevExpress.Mvvm;
using IDQ_Bot.Model.Struct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel : ViewModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public Profile SelectedProfile { get; set; }
        public ObservableCollection<Profile> ProfileList { get; set; }
    }
}
