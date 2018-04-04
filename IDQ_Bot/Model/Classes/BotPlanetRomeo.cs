using IDQ_Bot.Model.Struct;
using IDQ_Bot.Model.Structs;
using IDQ_Bot.Model.Structs.PlanetRomeo;
using IDQ_Core_0.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IDQ_Bot.Model.Classes
{
    class BotPlanetRomeo
    {
        private IMessageService _log;
        private IMessageService _error;
        private IWebDriverManager web;
        private bool _err;


        public BotPlanetRomeo(
            IMessageService log, IMessageService errorlog, IWebDriverManager webdriver)
        {
            _log = log;
            _error = errorlog;
            web = webdriver;
            web.Action += EventWebAction;
            web.ActionError += EventWebError;
        }

        public bool Authorization(string login, string password)
        {
            web.
                GoToUrl("https://www.planetromeo.com/#/auth/login").
                FindElement("a.login__signup-link").
                Click().
                FindElement("input#id_username").
                SendKeys(login).
                FindElement("input#id_password").
                SendKeys(password).
                Sleep(3000).
                FindElement("button.mb").
                Click().Sleep(5000);

            return IsAuthorized();
        }
        public void LogOut()
        {
            web.GoToUrl("https://classic.planetromeo.com/main/logout/");
        }
        public void Quit()
        {
            web.Quit();
        }

        public void SearchProfilesGay(PRSearchSettings settings, int mintimeout, int maxtimeout)
        {
            web.GoToUrl("https://classic.planetromeo.com/").
                SwitchFrame("rechts").
                FindElement("a[href=\"/search/?action=showForm&searchType=userDetail\"]").
                Sleep(1500, 2000).
                Click().
                SwitchToDefault().
                SwitchFrame("mitte").
                SelectOption("select[name=\"continent\"]", settings.Continent).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"country\"]", settings.Country).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"area\"]", settings.Area).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"alter1\"]", settings.MinAge).
                Sleep(mintimeout, maxtimeout).
                SelectOption("select[name=\"alter2\"]", settings.MaxAge).
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
        public bool SendMessage(PRProfileGay gay, List<Message> messages, int mintime, int maxtime)
        {
            _err = false;

            var listmess = new List<Message>();

            foreach (var item in messages)
            {
                listmess.Add(new Message { Text = item.Text.Replace("<nick>", gay.NickName) });
            }

            web.GoToUrl(gay.LinkProfile).
                FindElement("a[href*=\"/msg/?uid\"]")
                .Sleep(mintime, maxtime).
                Click().SwitcWindow(web.GetWindows().Last()).
                FindElement("textarea#txt").
                Sleep(mintime, maxtime).
                SendKeys(listmess[RandInt(0, messages.Count - 1)].Text).
                Sleep(mintime, maxtime).
                FindElement("input#id_submit").Click().SwitcWindow(web.GetWindows().Last());

            return _err;
        }
        public int SendMessageEmail(PRProfileGay gay, List<Message> messages, int mintime, int maxtime)
        {
            _err = false;
            int status = 0;

            var listmess = new List<Message>();

            foreach (var item in messages)
            {
                listmess.Add(new Message { Text = item.Text.Replace("<nick>", gay.NickName) });
            }

            web.GoToUrl(gay.LinkProfile).
                FindElement("a[href*=\"/msg/history.php?\"]")
                .Sleep(mintime, maxtime).
                Click().SwitcWindow(web.GetWindows().Last());
            List<IWebElement> elements = web.FindElements("a[href*=\"auswertung/setcard/?set=\"]").WebElements;

            if(elements.Last().Text == gay.NickName)
            {
                web.GoToUrl(gay.LinkMessage).
                FindElement("textarea#txt").
                Sleep(mintime, maxtime).
                SendKeys(listmess[RandInt(0, messages.Count - 1)].Text).
                Sleep(mintime, maxtime).
                FindElement("input#id_submit").Click().SwitcWindow(web.GetWindows().First());
                status = 1;
            }
            else
            {
                web.SwitcWindow(web.GetWindows().First());
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
        public void AnswerMessages()
        {
            web.GoToUrl("https://classic.planetromeo.com/").
                SwitchToDefault().
                SwitchFrame("unten").
                FindElement("a[href*=\"/mitglieder/messages/\"]").
                Click().
                SwitchToDefault().
                SwitchFrame("mitte");
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

            string pattern = "\\?uid=(\\d+)";
            Regex regex = new Regex(pattern);
            
            for(int count = FromPage-1; count < ToPage; count ++)
            {
                if (_err) break;
                if(count > 0)
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
                        if(!(listprofiles.Find((x)=> x.UserID == Regex.Match(linksMessage[i].GetAttribute("href"), pattern).Groups[1].Value).UserID ==
                            Regex.Match(linksMessage[i].GetAttribute("href"), pattern).Groups[1].Value))
                        listprofiles.Add(new PRProfileGay(
                            Regex.Match(linksMessage[i].GetAttribute("href"), pattern).Groups[1].Value,
                            linksProfile[i].Text,
                            linksProfile[i].GetAttribute("href"),
                            linksMessage[i].GetAttribute("href"),
                            StatusProfileGay.NotSend));
                    }
                }
                catch { }
            }
            listout = listprofiles;
            return _err;
        }

        private bool IsAuthorized()
        {
            web.GoToUrl("https://www.planetromeo.com/");
            return web.IsElementPresent("img.avatar__image");
        }
        private void EventWebError(string obj)
        {
            _error.ShowError(obj);
            _err = true;
        }
        private void EventWebAction(string obj)
        {
            _log.ShowMesssage(obj);
        }
        private int RandInt(int min, int max)
        {
            return new Random().Next(min, max);
        }
    }
}
