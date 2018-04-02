using DevExpress.Mvvm;
using IDQ_Bot.Model.Structs;
using IDQ_Core_0.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
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
    partial class PRViewModel : ViewModelBase
    {
        public DelegateCommand UpButton
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    int temp = Int32.Parse(CountMessage) + 1;
                    CountMessage = temp.ToString();
                    RaisePropertyChanged("CountMessage");
                });
            }
        }
        public DelegateCommand DownButton
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    int temp = Int32.Parse(CountMessage);

                    if (temp > 1)
                    {
                        temp--;
                        CountMessage = temp.ToString();
                        RaisePropertyChanged("CountMessage");
                    }
                });
            }
        }
        public DelegateCommand AddMessage
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!ListMessages.Contains(new Message { Text = Message }) && Message != null && Message != "")
                    {
                        ListMessages.Add(new Message { TypeMessage = SelectedTypeMessage, Text = Message });
                        Message = "";
                        SelectedMessage = ListMessages.Last();
                        msettings.SerializeToFile(PRMSETPATH);
                        RaisePropertiesChanged("SelectedMessage", "Message");
                    }
                });
            }
        }
        public DelegateCommand DeleteMessage
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (ListMessages.Contains(SelectedMessage))
                    {
                        ListMessages.Remove(SelectedMessage);
                        if (ListMessages.Count > 0)
                        {
                            SelectedMessage = ListMessages.Last();
                        }
                    }
                    msettings.SerializeToFile(PRMSETPATH);
                    RaisePropertiesChanged("SelectedMessage");
                }, () => ListMessages.Count > 0);
            }
        }
        //***************************************************************************************************
        private const string PRMSETPATH = "Settings/PlanetRomeo/MessagesSettings.json";
        private MessagesSettings msettings;
        private string _countMessage;
        private List<string> _listTypeMessage;
        private string _selectedTypeMessage;

        public string Message { get; set; }
        public ObservableCollection<Message> ListMessages { get; set; }
        public List<string> ListTypeMessage { get => _listTypeMessage; set => _listTypeMessage = value; }
        public string SelectedTypeMessage {
            get => _selectedTypeMessage;
            set => _selectedTypeMessage = value; }
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

        private void InitMessageSettings()
        {
            if (FileManager.IsExist(PRMSETPATH))
            {
                msettings = JSONSerializer.Deserialize<MessagesSettings>(FileManager.ReadTextFromFile(PRMSETPATH));
                ListMessages = msettings.ListMessages;
                SelectedMessage = msettings.SelectedMessage;
            }
            else
            {
                ListMessages = new ObservableCollection<Message>();
                ListMessages.Add(new Message { TypeMessage= "FirstMessage", Text = "Test Message" });
                msettings = new MessagesSettings
                {
                    ListMessages = ListMessages,
                    SelectedMessage = SelectedMessage
                };
                msettings.SerializeToFile(PRMSETPATH);
            }
            CountMessage = "1";

            ListTypeMessage = new List<string>
            {
                "FirstMessage",
                "EmailMessage",
                "ThanksMessage"
            };
            SelectedTypeMessage = ListTypeMessage.First();
        }
    }
}
