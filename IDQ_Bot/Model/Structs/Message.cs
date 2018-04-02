using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Structs
{
    enum TypeMessage
    {
        FirstMessage,
        EmailMessage,
        ThanksMessage
    }

    struct Message
    {
        private TypeMessage _typeMessage;

        public TypeMessage Type { get => _typeMessage; set => _typeMessage = value; }
        public string TypeMessage
        {
            get => _typeMessage.ToString();
            set
            {
                switch (value)
                {
                    case "FirstMessage":
                        {
                            _typeMessage = Structs.TypeMessage.FirstMessage;
                            break;
                        }
                    case "EmailMessage":
                        {
                            _typeMessage = Structs.TypeMessage.EmailMessage;
                            break;
                        }
                    case "ThanksMessage":
                        {
                            _typeMessage = Structs.TypeMessage.ThanksMessage;
                            break;
                        }
                    default:
                        {
                            _typeMessage = Structs.TypeMessage.FirstMessage;
                            break;
                        }

                }
            }
        }
        public string Text { get; set; }

        public Message(List<string> messages) : this()
        {
        }
    }
}
