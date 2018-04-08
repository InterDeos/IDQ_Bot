using System;
using IDQ_Core_0.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_DataModel.Main.Interface
{
    public enum TypeError
    {

    }

    interface IBotID: IBot
    {

        event Action<string> ActionEvent;
        event Action<string> ActionErrorEvent;

        bool IsAuthorized();
        bool Authorization(string login, string password);
        bool Logout();
        void Quit();
    }
}
