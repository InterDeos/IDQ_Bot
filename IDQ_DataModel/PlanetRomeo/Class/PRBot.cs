using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IDQ_Core_0.Interface;
using IDQ_DataModel.Main.Interface;
using IDQ_DataModel.Main.Struct;
using IDQ_DataModel.PlanetRomeo.Struct;
using OpenQA.Selenium;

namespace IDQ_DataModel.PlanetRomeo.Class
{
    public class PRBot : IBotID
    {
        private readonly IWebDriverManager web;
        private bool _err;

        private void Web_Action(string obj)
        {
            ActionEvent?.Invoke(obj);
        }
        private void Web_ActionError(string obj)
        {
            ActionErrorEvent?.Invoke(obj);
        }


        private int RandInt(int min, int max)
        {
            if (max == 0 && min == 0) return 0;
            if (min > max) return min;
            return new Random().Next(min, max);
        }

        public PRBot(IWebDriverManager webdriver)
        {
            web = webdriver;

            web.Action += Web_Action;
            web.ActionError += Web_ActionError;
            
            Work = false;
        }
        ~PRBot()
        {
            ActionEvent?.Invoke("Stop");
        }
        

        public bool Work { get; private set; }

        public event Action<string> ActionEvent;
        public event Action<string> ActionErrorEvent;

        public void Execute(Action<IWebDriverManager> action)
        {
            Work = true;
            action(web);
            Work = false;
        }

        public bool IsAuthorized()
        {
            web.SwitchToDefault().
                SwitchFrame("persoenliches");
            if (web.FindElementObj("div.welcome_text") == null) return false;
            else return true;
        }
        public bool Authorization(string login, string password)
        {
            Work = true;
            web.
                GoToUrl("https://classic.planetromeo.com/#/auth/login").
                SwitchFrame("persoenliches").
                FindElement("input#id_username").
                SendKeys(login).
                FindElement("input#id_password").
                SendKeys(password).
                Sleep(3000).
                FindElement("input[type=\"submit\"]").
                Click().Sleep(5000).SwitchToDefault();

            Work = false;

            return IsAuthorized();
        }
        public bool Logout()
        {
            web.GoToUrl("https://classic.planetromeo.com/main/logout/");
            return true;
        }
        public void Quit()
        {
            web.Quit();
        }

        public void SearchProfilesGay(PRSearchSettings settings, int mintimeout, int maxtimeout)
        {
            SearchProfilesGay(settings.Continent, settings.Country, settings.Area,
                settings.MinAge, settings.MaxAge, mintimeout, maxtimeout);
        }
        public void SearchProfilesGay(string continent, string country, 
            string area, string minAge, string maxAge, int mintimeout, 
            int maxtimeout)
        {
            web.SwitchToDefault().
                SwitchFrame("rechts").
                FindElement("a[href=\"/search/?action=showForm&searchType=userDetail\"]").
                Sleep(1500, 2000).
                Click().
                SwitchToDefault().
                SwitchFrame("mitte").
                SelectOption("select[name=\"continent\"]", continent).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"country\"]", country).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"area\"]", area).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"alter1\"]", minAge).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"alter2\"]", maxAge).
                FindElement("input#id_besuch").
                Click().
                FindElement("img[src=\"/v1/img/32/devil.png\"]").
                Click().
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"sex[]\"]", "1").
                Sleep(mintimeout, maxtimeout).
                FindElement("input#id_submit").
                Click().
                Sleep(mintimeout, maxtimeout);
        }

        public bool ParseProfiles(int FromPage, int ToPage, int mintime, int maxtime, List<PRProfileGay> list, out List<PRProfileGay> listout)
        {
            _err = false;
            var listprofiles = list;

            web.SwitchToDefault().
                SwitchFrame("mitte").
                FindElements("a[href^=\"?action=showPage&searchType=userDetail&searchResultId");

            List<string> pages = new List<string>();
            foreach (var item in web.WebElements)
            {
                pages.Add(item.GetAttribute("href"));
                if (pages.Count >= 20) break;
            }

            pages = pages.Distinct().ToList();

            foreach(var it in pages)
            {
                Web_Action(string.Format("Count: {0} href = {1}", pages.Count, it));
            }

            string pattern = "\\?uid=(\\d+)";
            Regex regex = new Regex(pattern);
            var idlist = new List<string>();
            foreach(var item in listprofiles)
            {
                idlist.Add(item.UserID);
            }

            ToPage = ToPage > pages.Count ? pages.Count : ToPage;

            for (int count = FromPage - 1; count <= ToPage; count++)
            {
                if (_err) break;
                if (count > 0)
                {
                    web.GoToUrl(pages[count-1]).Sleep(mintime, maxtime);
                }
                try
                {
                    List<IWebElement> linksProfile = web.FindElementsObj("a.profile-name").ToList();
                    List<IWebElement> optionLines = web.FindElementsObj("td.optionLine > span[style*=\"white-space:nowrap\"]").ToList();
                    List<IWebElement> linksMessage = web.FindElementsObj("a[href^=\"../msg/?uid=\"]").ToList();

                    for (int i = 0; i < linksProfile.Count; i++)
                    {
                        string id = Regex.Match(linksMessage[i].GetAttribute("href"), pattern).Groups[1].Value;
                        
                        if (!idlist.Contains(id))
                            listprofiles.Add(new PRProfileGay(
                                id,
                                linksProfile[i].Text,
                                linksProfile[i].GetAttribute("href"),
                                linksMessage[i].GetAttribute("href"),
                                StatusProfileGay.NotSend));
                        idlist.Add(id);
                    }
                }
                catch { _err = true; break; }
            }
            listout = listprofiles;
            return _err;
        }

        private List<string> GetLanguages()
        {
            List<string> listLanguage = new List<string>();
            
            var tdList = web.FindElementsObj("table.prfl > tbody > tr > td").ToList();

            if (tdList.FindIndex((x) => x.GetAttribute("class") == "headline") >= 0)
            {
                tdList.RemoveAt(tdList.FindIndex((x) => x.GetAttribute("class") == "headline"));
            }

            return tdList[0].Text.Split(' ').ToList();
        }

        public bool SendMessage(PRProfileGay gay, List<Message> messages, int mintime, int maxtime)
        {
            _err = false;

            var listmess = new List<Message>();
            
            foreach (var item in messages)
            {
                listmess.Add(new Message { Text = item.Text.Contains("<nick>") ? item.Text.Replace("<nick>", gay.NickName) + ";" + item.Language : item.Text + ";" + item.Language });
            }

            web.GoToUrl(gay.LinkProfile).Sleep(mintime, maxtime * 2);

            var thList = GetLanguages();
            var messs = new List<Message>();

            foreach (var item in thList)
            {
                string lang = item.Split(',')[0];
                messs.AddRange(listmess.Where((x)=> x.Language == lang));
                Web_Action(string.Format("Messs count: {0} = {1}",lang,messs.Count));
            }
            if (messs.Count == 0)
            {
                Web_ActionError("Langueges not found in ListMessage!\n Search English");
                messs.AddRange(listmess.Where((x) => x.Language == "English"));
                if (messs.Count == 0) { Web_ActionError("English Messages not found!"); return true; }
            }

            

            web.FindElement("a[href*=\"/msg/history.php?\"]")
                .Sleep(mintime, maxtime).
                Click().SwitcWindow(web.GetWindows().Last()).Sleep(mintime, maxtime);

            if (!web.IsElementPresent("div#humaninputcheck"))
            {
                web.GoToUrl(gay.LinkMessage);
                web.FindElement("textarea#txt");
                web.Sleep(mintime, maxtime);
                int ind = RandInt(0, messs.Count - 1);
                if (ind >= messs.Count) ind =  messs.Count -1 ;
                web.SendKeys(messs[ind].Text);
                web.Sleep(mintime, maxtime);
                List<string> vs = web.GetWindows();
                web.FindElement("input#id_submit");
                web.Sleep(500);
                web.Click();
                web.Sleep(500);
                web.SwitcWindow(vs.First());
            }
            else
            {
                _err = true;
            }

            return _err;
        }
        public int SendMessageEmail(PRProfileGay gay, List<Message> messages, int mintime, int maxtime)
        {
            int status = 0;

            _err = false;

            var listmess = new List<Message>();

            foreach (var item in messages)
            {
                listmess.Add(new Message { Text = item.Text.Contains("<nick>") ? item.Text.Replace("<nick>", gay.NickName) + ";" + item.Language : item.Text + ";" + item.Language });
            }

            web.GoToUrl(gay.LinkProfile).Sleep(mintime, maxtime * 2);

            var thList = GetLanguages();
            var messs = new List<Message>();

            foreach (var item in thList)
            {
                string lang = item.Split(',')[0];
                messs.AddRange(listmess.Where((x) => x.Language == lang));
                Web_Action(string.Format("Messs count: {0} = {1}", lang, messs.Count));
            }
            if (messs.Count == 0)
            {
                Web_ActionError("Langueges not found in ListMessage!\n Search English");
                messs.AddRange(listmess.Where((x) => x.Language == "English"));
                if (messs.Count == 0) { Web_ActionError("English Messages not found!"); return 0; }
            }


            web.FindElement("a[href*=\"/msg/history.php?\"]")
                .Sleep(mintime, maxtime).
                Click().SwitcWindow(web.GetWindows().Last()).Sleep(mintime, maxtime);
            List<IWebElement> elements = web.FindElements("a[href*=\"auswertung/setcard/?set=\"]").WebElements;

            if (elements.Count > 0 && elements.Last().Text == gay.NickName)
            {
                web.GoToUrl(gay.LinkMessage);
                web.FindElement("textarea#txt");
                web.Sleep(mintime, maxtime);
                int ind = RandInt(0, messs.Count - 1);
                if (ind >= messs.Count) ind = messs.Count - 1;
                web.SendKeys(messs[ind].Text);
                web.Sleep(mintime, maxtime);
                List<string> vs = web.GetWindows();
                web.FindElement("input#id_submit");
                web.Sleep(500);
                web.Click();
                web.Sleep(500);
                web.SwitcWindow(vs.First());
                status = 1;
            }
            else
            {
                web.Close().SwitcWindow(web.GetWindows().First());
                status = 2;
            }

            if (_err)
            {
                return 0;
            }
            else
            {
                return status;
            }
        }
    }
}
