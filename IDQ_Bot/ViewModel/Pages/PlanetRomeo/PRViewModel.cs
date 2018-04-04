using DevExpress.Mvvm;
using IDQ_Bot.Model.Classes;
using IDQ_Bot.Model.Classes.PlanetRomeo;
using IDQ_Bot.Model.Struct;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Classes;
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

        public ObservableCollection<PRProfileGay> ListProfileGays { get; set; }
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
                RaisePropertiesChanged("ListProfileGays");
            }
            else
            {
                ListProfileGays = new ObservableCollection<PRProfileGay>();
            }

        }




        private void Parse(BotPlanetRomeo bot, ref List<PRProfileGay> list)
        {
            bot.SearchProfilesGay(SelectedSearchSettings.ToID(places), MinTimeOutOfSteps, MaxTimeOutOfSteps);
            var listout = new List<PRProfileGay>();
            _err = bot.ParseProfiles(FromCountParsePage, ToCountParsePage, MinTimeOutOfSteps, MaxTimeOutOfSteps, list, out list);
        }
        private void Send(BotPlanetRomeo bot, ref List<PRProfileGay> list)
        {
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

                            if (!bot.SendMessage(gay, ListMessages.Where((x) => x.Type == Model.Structs.TypeMessage.FirstMessage).ToList(), MinTimeOutOfSteps, MaxTimeOutOfSteps))
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

                            int status = bot.SendMessageEmail(gay, ListMessages.Where((x) => x.Type == Model.Structs.TypeMessage.EmailMessage).ToList(), MinTimeOutOfSteps, MaxTimeOutOfSteps);

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

        private void SaveList(List<PRProfileGay> list)
        {
            list.SerializeToFile(PARSEDPATH);
            ListProfileGays = new ObservableCollection<PRProfileGay>(list);
            if (ListProfileGays.Count > 0) SelectedProfileGay = ListProfileGays.Last();
            RaisePropertyChanged("ListProfileGays");
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
                return new DelegateCommand(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        _work = true;
                        var bot = new BotPlanetRomeo(new ConsoleMessageService(), new WinFormMessageService(),
                            new WebDriverManager(true));

                        if (bot.Authorization(SelectedProfile.Login, SelectedProfile.Password))
                        {
                            Parse(bot, ref listprofilegays);
                            if (!_err)
                            {
                                bot.LogOut();
                                bot.Quit();
                            }
                            SaveList(listprofilegays);
                        }
                        new WinFormMessageService().ShowMesssage("END PARSE PROFILES");
                        _work = false;
                    });
                }, () =>
                {
                    if (!_work && SelectedProfile != default(Profile) && SelectedSearchSettings != default(PRSearchSettings) && ListMessages.Count > 0) return true; else return false;
                });
            }
        }
        public DelegateCommand SendMessages
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        _work = true;
                        var bot = new BotPlanetRomeo(new ConsoleMessageService(), new WinFormMessageService(),
                            new WebDriverManager(true));

                        if (bot.Authorization(SelectedProfile.Login, SelectedProfile.Password))
                        {
                            listprofilegays = ListProfileGays.ToList();
                            Send(bot, ref listprofilegays);
                            if (!_err)
                            {
                                bot.LogOut();
                                bot.Quit();
                            }

                            SaveList(listprofilegays);
                        }
                        new WinFormMessageService().ShowMesssage("END SEND MESSAGE");
                        _work = false;
                    });
                }, () =>
                {
                    if (!_work && SelectedProfile != default(Profile) && SelectedSearchSettings != default(PRSearchSettings) && ListMessages.Count > 0) return true; else return false;
                });
            }
        }

        public DelegateCommand ParseAndSend
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        _work = true;
                        var bot = new BotPlanetRomeo(new ConsoleMessageService(), new WinFormMessageService(),
                            new WebDriverManager(true));
                        if (bot.Authorization(SelectedProfile.Login, SelectedProfile.Password))
                        {
                            Parse(bot, ref listprofilegays);
                            SaveList(listprofilegays);
                            if (!_err) Send(bot, ref listprofilegays);
                            SaveList(listprofilegays);

                            if (!_err)
                            {
                                bot.LogOut();
                                bot.Quit();
                            }
                        }
                        new WinFormMessageService().ShowMesssage("END PARSE AND SEND MESSAGE");
                        _work = false;
                    });
                }, () =>
                {
                    if (!_work && SelectedProfile != default(Profile) && SelectedSearchSettings != default(PRSearchSettings) && ListMessages.Count > 0) return true; else return false;
                });
            }
        }
    }
}
