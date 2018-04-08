using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_DataModel.PlanetRomeo.Struct
{
    public enum StatusProfileGay
    {
        NotSend,
        FistMessage,
        EmailMessage,
    }
    public struct PRProfileGay
    {
        private StatusProfileGay _status;

        public string UserID { get; set; }
        public string NickName { get; set; }
        public string LinkProfile { get; set; }
        public string LinkMessage { get; set; }
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
