using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Struct
{
    struct Profile
    {
        private string _login;
        private string _password;
        private string _language;

        public string Login { get => _login; set => _login = value; }
        public string Password { get => _password; set => _password = value; }
        public string Language { get => _language; set => _language = value; }

        public Profile(string login, string password, string language) : this()
        {
            Login = login;
            Password = password;
            Language = language;
        }

        public static bool operator ==(Profile op1, Profile op2)
        {
            return op1.Equals(op2);
        }
        public static bool operator !=(Profile op1, Profile op2)
        {
            return !op1.Equals(op2);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
