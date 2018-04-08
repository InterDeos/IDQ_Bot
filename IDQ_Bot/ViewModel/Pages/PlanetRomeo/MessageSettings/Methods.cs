using DevExpress.Mvvm;
using IDQ_Bot.Model.Classes.PlanetRomeo;
using IDQ_Bot.Model.Structs;
using IDQ_Core_0.Class;
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
                ListMessages.Add(new Message { TypeMessage = "FirstMessage", Text = "Test Message" });
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
                    string text = "";

                    if (Message.Split(';').Count() > 1)
                    {
                        text = Message;
                    }
                    else { text = Message + ";" + "English"; }

                    if (!ListMessages.Contains(new Message { Text = text }) && Message != "")
                    {
                        ListMessages.Add(new Message { TypeMessage = SelectedTypeMessage, Text = text });
                        Message = "";
                        SelectedMessage = ListMessages.Last();
                        msettings.SerializeToFile(PRMSETPATH);
                        RaisePropertiesChanged("SelectedMessage", "Message");
                    }
                },()=>
                {
                    return (Message != null && Message != "");
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
    }
}
