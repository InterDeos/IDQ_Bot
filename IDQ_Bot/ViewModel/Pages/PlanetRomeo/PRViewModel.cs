using DevExpress.Mvvm;
using IDQ_Bot.Model.Classes;
using IDQ_Bot.Model.Struct;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel : ViewModelBase
    {
        private bool _work = false;
        private const string PARSEDPATH = "Data/PlanetRomeo/ParsedProfilesGay.json";

        public ObservableCollection<PRProfileGay> ListProfileGays { get; set; }

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
                            bot.SearchProfilesGay(SelectedSearchSettings.ToID(places), MinTimeOutOfSteps, MaxTimeOutOfSteps);
                            List<PRProfileGay> result = bot.ParseProfiles(CountParsePage, MinTimeOutOfSteps, MaxTimeOutOfSteps);
                            bot.LogOut();
                            bot.Quit();
                            result.SerializeToFile(PARSEDPATH);
                            ListProfileGays = new ObservableCollection<PRProfileGay>(result);
                            RaisePropertyChanged("ListProfileGays");
                        }
                        new WinFormMessageService().ShowMesssage("END PARSE PROFILES");
                        _work = false;
                    });
                }, () => {
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
                            List<PRProfileGay> gays = JSONSerializer.Deserialize<List<PRProfileGay>>(FileManager.ReadTextFromFile(PARSEDPATH));

                            var count = 0;
                            var countmessage = Int32.Parse(CountMessage);

                            List<PRProfileGay> deletelist = new List<PRProfileGay>();
                            foreach (var gay in gays.Where((x) => x.TypeStatus == StatusProfileGay.NotSend))
                            {
                                if (count >= countmessage) break;

                                bot.SendMessage(gay, ListMessages.ToList(), MinTimeOutOfSteps, MaxTimeOutOfSteps);
                                deletelist.Add(gay);
                                count++;
                            }


                            List<PRProfileGay> temp = new List<PRProfileGay>();
                            foreach(var item in deletelist)
                            {
                                gays.Remove(item);
                                temp.Add(new PRProfileGay(item.UserID, item.NickName, item.LinkProfile, item.LinkMessage, StatusProfileGay.FistMessage));
                            }
                            foreach(var item in gays)
                            {
                                temp.Add(item);
                            }

                            temp.SerializeToFile(PARSEDPATH);
                            ListProfileGays = new ObservableCollection<PRProfileGay>(temp);
                            RaisePropertiesChanged("ListProfileGays");
                            bot.LogOut();
                            bot.Quit();
                        }
                        new WinFormMessageService().ShowMesssage("END SEND MESSAGE");
                        _work = false;
                    });
                }, () => {
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
                            bot.SearchProfilesGay(SelectedSearchSettings.ToID(places), MinTimeOutOfSteps, MaxTimeOutOfSteps);
                            List<PRProfileGay> result = bot.ParseProfiles(CountParsePage, MinTimeOutOfSteps, MaxTimeOutOfSteps);
                            var m = new ConsoleMessageService();
                            foreach (var item in result)
                            {
                                m.ShowExclamation(string.Format("\nID: {0}\nNickName: {1}", item.UserID, item.NickName));
                            }
                            result.SerializeToFile(PARSEDPATH);

                            var count = 0;
                            var countmessage = Int32.Parse(CountMessage);

                            List<PRProfileGay> deletepro = new List<PRProfileGay>();
                            foreach (var gay in result.Where((x) => x.TypeStatus == StatusProfileGay.NotSend))
                            {
                                if (count >= countmessage) break;

                                bot.SendMessage(gay, ListMessages.ToList(), MinTimeOutOfSteps, MaxTimeOutOfSteps);
                                deletepro.Add(gay);
                                count++;
                            }
                            foreach (var item in deletepro)
                            {
                                result.Remove(item);
                                result.Add(new PRProfileGay(item.UserID, item.NickName, item.LinkProfile, item.LinkMessage, StatusProfileGay.FistMessage));
                            }

                            result.SerializeToFile(PARSEDPATH);
                            ListProfileGays = new ObservableCollection<PRProfileGay>(result);
                            RaisePropertiesChanged("ListProfileGays");
                            bot.LogOut();
                            bot.Quit();
                        }
                        new WinFormMessageService().ShowMesssage("END PARSE AND SEND MESSAGE");
                        _work = false;
                    });
                }, () => {
                    if (!_work && SelectedProfile != default(Profile) && SelectedSearchSettings != default(PRSearchSettings) && ListMessages.Count > 0) return true; else return false;
                });
            }
        }
    }
}
