using IDQ_Bot.Model.Structs;
using IDQ_DataModel.Main.Struct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Classes.PlanetRomeo
{
    class MessagesSettings
    {
        private ObservableCollection<Message> _listMessages;
        private Message _selectedMessage;

        public ObservableCollection<Message> ListMessages { get => _listMessages; set => _listMessages = value; }
        public Message SelectedMessage { get => _selectedMessage; set => _selectedMessage = value; }

        public MessagesSettings()
        {
            ListMessages = new ObservableCollection<Message>();
            SelectedMessage = new Message();
        }
    }
}
