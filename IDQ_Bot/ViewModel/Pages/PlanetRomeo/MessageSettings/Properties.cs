using IDQ_Bot.Model.Structs;
using IDQ_DataModel.Main.Struct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel
    {
        public string Message { get; set; }
        public ObservableCollection<Message> ListMessages { get; set; }
        public List<string> ListTypeMessage { get => _listTypeMessage; set => _listTypeMessage = value; }
        public string SelectedTypeMessage
        {
            get => _selectedTypeMessage;
            set => _selectedTypeMessage = value;
        }
        public Message SelectedMessage { get; set; }
        public string CountMessage
        {
            get => _countMessage;
            set
            {
                try
                {
                    var temp = Int32.Parse(value);
                    if (temp > 0) _countMessage = value; else _countMessage = "1";
                    RaisePropertyChanged("CountMessage");
                }
                catch
                {
                    _countMessage = "1";
                    RaisePropertyChanged("CountMessage");
                }
            }
        }
    }
}
