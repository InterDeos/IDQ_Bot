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
            bool result = false;

            web.GoToUrl(gay.LinkProfile).
                FindElement("a[href*=\"/msg/?uid\"")
                .Sleep(mintime, maxtime).
                Click().SwitcWindow(web.GetWindows().Last()).
                FindElement("textarea#txt").
                Sleep(mintime, maxtime).
                SendKeys(messages[RandInt(0, messages.Count - 1)].Text).
                Sleep(mintime, maxtime).
                FindElement("input#id_submit").Click().SwitcWindow(web.GetWindows().Last());

            return result;
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
        public List<PRProfileGay> ParseProfiles(int countPage, int mintime, int maxtime)
        {
            var listprofiles = new List<PRProfileGay>();

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

            var count = 0;
            foreach (string link in pages)
            {
                if (count < countPage)
                {
                    List<IWebElement> linksProfile = web.FindElementsObj("a.profile-name").ToList();
                    List<IWebElement> optionLines = web.FindElementsObj("td.optionLine > span[style*=\"white-space:nowrap\"]").ToList();
                    List<IWebElement> linksMessage = web.FindElementsObj("a[href^=\"../msg/?uid=\"]").ToList();

                    for (int i = 0; i < linksProfile.Count; i++)
                    {
                        listprofiles.Add(new PRProfileGay(
                            Regex.Match(linksMessage[i].GetAttribute("href"), pattern).Groups[1].Value,
                            linksProfile[i].Text,
                            linksProfile[i].GetAttribute("href"),
                            linksMessage[i].GetAttribute("href"),
                            StatusProfileGay.NotSend));
                    }

                    web.GoToUrl(link).Sleep(mintime, maxtime);
                }
                count++;
            }
            return listprofiles;
        }

        private bool IsAuthorized()
        {
            web.GoToUrl("https://www.planetromeo.com/");
            return web.IsElementPresent("img.avatar__image");
        }
        private void EventWebError(string obj)
        {
            _error.ShowError(obj);
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
