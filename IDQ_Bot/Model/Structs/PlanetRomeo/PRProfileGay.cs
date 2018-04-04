using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Structs.PlanetRomeo
{
    enum StatusProfileGay
    {
        NotSend,
        FistMessage,
        EmailMessage,
    }
    struct PRProfileGay
    {
        private string _userID;
        private string _nickName;
        private string _linkProfile;
        private string _linkMessage;
        private StatusProfileGay _status;

        public string UserID { get => _userID; set => _userID = value; }
        public string NickName { get => _nickName; set => _nickName = value; }
        public string LinkProfile { get => _linkProfile; set => _linkProfile = value; }
        public string LinkMessage { get => _linkMessage; set => _linkMessage = value; }
        public string Status
        {
            get => _status.ToString();
            set
            {
                switch (value)
                {
                    case "NotSend":
                        {
                            _status = StatusProfileGay.NotSend;
                            break;
                        }
                    case "FistMessage":
                        {
                            _status = StatusProfileGay.FistMessage;
                            break;
                        }
                    case "EmailMessage":
                        {
                            _status = StatusProfileGay.EmailMessage;
                            break;
                        }
                }
            }
        }
        public StatusProfileGay TypeStatus { get => _status; set => _status = value; }

        public PRProfileGay(string userID, string nick, string linkPro, string linkMessage, StatusProfileGay status) : this()
        {
            UserID = userID;
            NickName = nick;
            LinkProfile = linkPro;
            LinkMessage = linkMessage;
            _status = status;
        }
    }
}
