using DevExpress.Mvvm;
using IDQ_Bot.Model.Classes;
using IDQ_Bot.Model.Classes.PlanetRomeo;
using IDQ_Bot.Model.Struct;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Class;
using IDQ_Core_0.Class.MessageService;
using IDQ_DataModel.Main.Struct;
using IDQ_DataModel.PlanetRomeo.Struct;
using IDQ_DataModel.PlanetRomeo.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel : ViewModelBase
    {
        private bool _work = false;
        private bool _err = false;
        private const string PARSEDPATH = "Data/PlanetRomeo/ParsedProfilesGay.json";
        private List<PRProfileGay> listprofilegays = new List<PRProfileGay>();
        private PRProfileGay _selectedProfileGay;
        private PRBot bot;
        private ConsoleMessageService log;

        public ObservableCollection<PRProfileGay> ListProfileGays { get; set; }
        public string LabelListProfiles { get; set; }
        public PRProfileGay SelectedProfileGay { get => _selectedProfileGay; set => _selectedProfileGay = value; }

        public PRViewModel()
        {
            InitProfileSettings();
            InitSearchSettings();
            InitParseSettings();
            InitMessageSettings();

            if (FileManager.IsExist(PARSEDPATH))
            {
                List<PRProfileGay> list = JSONSerializer.DeserializeFromFile<List<PRProfileGay>>(PARSEDPATH);
                ListProfileGays = new ObservableCollection<PRProfileGay>(list);
                listprofilegays = list;
                RaisePropertiesChanged("ListProfileGays");
                LabelListProfiles = string.Format("List Profiles Gays Count: {0}", ListProfileGays.Count);
                RaisePropertyChanged("LabelListProfiles");
            }
            else
            {
                ListProfileGays = new ObservableCollection<PRProfileGay>();
                LabelListProfiles = string.Format("List Profiles Gays Count: {0}", ListProfileGays.Count);
                RaisePropertyChanged("LabelListProfiles");
            }

            log = new ConsoleMessageService();
            bot = new PRBot(new WebDriverManager(), log, log);
        }




        private void Parse(ref List<PRProfileGay> list)
        {
            bot.SearchProfilesGay(SelectedSearchSettings.ToID(places), MinTimeOutOfSteps, MaxTimeOutOfSteps);
            _err = bot.ParseProfiles(FromCountParsePage, ToCountParsePage, MinTimeOutOfSteps, MaxTimeOutOfSteps, list, out list);
        }
        private void Send(ref List<PRProfileGay> list)
        {
            _err = false;
            switch (SelectedTypeMessage)
            {
                case "FirstMessage":
                    {
                        var count = 0;
                        var countmessage = Int32.Parse(CountMessage);
                        List<PRProfileGay> deletelist = new List<PRProfileGay>();

                        foreach (var gay in list.Where((x) => x.TypeStatus == StatusProfileGay.NotSend))
                        {
                            if (count >= countmessage) break;
                            if (_err) break;

                            if (!bot.SendMessage(gay, ListMessages.Where((x) => x.Type == TypeMessage.FirstMessage).ToList(), MinTimeOutOfSteps, MaxTimeOutOfSteps))
                            {
                                deletelist.Add(gay);
                                count++;
                            }
                            else
                            {
                                _err = true;
                                break;
                            }

                        }

                        List<PRProfileGay> temp = new List<PRProfileGay>();
                        foreach (var item in deletelist)
                        {
                            list.Remove(item);
                            temp.Add(new PRProfileGay(item.UserID, item.NickName, item.LinkProfile, item.LinkMessage, StatusProfileGay.FistMessage));
                        }
                        foreach (var item in list)
                        {
                            temp.Add(item);
                        }
                        list = temp;

                        break;
                    }
                case "EmailMessage":
                    {
                        var count = 0;
                        var countmessage = Int32.Parse(CountMessage);
                        List<PRProfileGay> deletelist = new List<PRProfileGay>();

                        foreach (var gay in list.Where((x) => x.TypeStatus == StatusProfileGay.FistMessage))
                        {
                            if (count >= countmessage) break;
                            if (_err) break;

                            int status = bot.SendMessageEmail(gay, ListMessages.Where((x) => x.Type == TypeMessage.EmailMessage).ToList(), MinTimeOutOfSteps, MaxTimeOutOfSteps);

                            if (status == 1)
                            {
                                deletelist.Add(gay);
                                count++;
                            }
                            else
                            {
                                if (status == 0)
                                {
                                    _err = true;
                                    break;
                                }
                                count++;
                            }
                        }

                        List<PRProfileGay> temp = new List<PRProfileGay>();
                        foreach (var item in deletelist)
                        {
                            list.Remove(item);
                            temp.Add(new PRProfileGay(item.UserID, item.NickName, item.LinkProfile, item.LinkMessage, StatusProfileGay.EmailMessage));
                        }
                        foreach (var item in list)
                        {
                            temp.Add(item);
                        }
                        list = temp;
                        break;
                    }
                case "ThanksMessage":
                    {

                        break;
                    }
            }
        }
        private bool UseCommand()
        {
            if (!_work && SelectedProfile != default(Profile) && SelectedSearchSettings != default(PRSearchSettings) && ListMessages.Count > 0)
                return true;
            else
                return false;
        }
        private Action HeadActions(Action action, string message)
        {
            return () =>
            {
                Task.Factory.StartNew(() =>
                {
                    _work = true;
                    bot = new PRBot(new WebDriverManager(true), log, log);
                    bot.ActionEvent += Bot_ActionEvent;
                    bot.ActionErrorEvent += Bot_ActionErrorEvent;

                    if (bot.Authorization(SelectedProfile.Login, SelectedProfile.Password))
                    {
                        action();

                        if (!_err)
                        {
                            bot.Logout();
                            bot.Quit();
                        }

                    }
                    WinFormMessageService.ShowMesssage(message);

                    _work = false;
                });
            };
        }

        private void Bot_ActionErrorEvent(string obj)
        {
            WinFormMessageService.ShowError(obj);
        }

        private void Bot_ActionEvent(string obj)
        {
            if(obj == "Stop") { _err = true; SaveList(listprofilegays); }
        }

        private void SaveList(List<PRProfileGay> list)
        {
            list.SerializeToFile(PARSEDPATH);
            ListProfileGays = new ObservableCollection<PRProfileGay>(list);
            if (ListProfileGays.Count > 0) SelectedProfileGay = ListProfileGays.Last();
            RaisePropertyChanged("ListProfileGays");
            LabelListProfiles = string.Format("List Profiles Gays Count: {0}", ListProfileGays.Count);
            RaisePropertyChanged("LabelListProfiles");
        }

        public DelegateCommand DeleteProfileGay
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ListProfileGays.Remove(SelectedProfileGay);
                    listprofilegays = ListProfileGays.ToList();
                    SaveList(listprofilegays);
                }, () => ListProfileGays.Count > 0);
            }
        }
        public DelegateCommand ClearList 
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    listprofilegays = new List<PRProfileGay>();
                    SaveList(listprofilegays);
                });
            }
        }

        public DelegateCommand ParseGays
        {
            get
            {
                return new DelegateCommand(HeadActions(() =>
                {
                    Parse(ref listprofilegays);
                    SaveList(listprofilegays);

                }, "END PARSE PROFILES"), UseCommand);
            }
        }
        public DelegateCommand SendMessages
        {
            get
            {
                return new DelegateCommand(HeadActions(()=>
                {
                    listprofilegays = ListProfileGays.ToList();
                    Send(ref listprofilegays);
                    SaveList(listprofilegays);

                }, "END SEND MESSAGE"), UseCommand);
            }
        }
        public DelegateCommand ParseAndSend
        {
            get
            {
                return new DelegateCommand(HeadActions(()=>
                {
                    Parse(ref listprofilegays);
                    SaveList(listprofilegays);
                    if (!_err) Send(ref listprofilegays);
                    SaveList(listprofilegays);

                }, "END PARSE AND SEND MESSAGE"), UseCommand);
            }
        }
    }
}
