using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_DataModel.Main.Struct
{
    public enum TypeMessage
    {
        FirstMessage,
        EmailMessage,
        ThanksMessage
    }

    public struct Message
    {
        private TypeMessage _typeMessage;
        private string _text;

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
                            _typeMessage = IDQ_DataModel.Main.Struct.TypeMessage.FirstMessage;
                            break;
                        }
                    case "EmailMessage":
                        {
                            _typeMessage = IDQ_DataModel.Main.Struct.TypeMessage.EmailMessage;
                            break;
                        }
                    case "ThanksMessage":
                        {
                            _typeMessage = IDQ_DataModel.Main.Struct.TypeMessage.ThanksMessage;
                            break;
                        }
                    default:
                        {
                            _typeMessage = IDQ_DataModel.Main.Struct.TypeMessage.FirstMessage;
                            break;
                        }

                }
            }
        }
        public string Language { get; set; }
        public string Text
        {
            get => _text;
            set
            {
                if (value != null && value.Split(';').Count() > 1) { Language = value.Split(';')[1]; _text = value.Split(';')[0]; }
                else
                    _text = value;
            }
        }

        public Message(List<string> messages) : this()
        {
        }
    }
}
