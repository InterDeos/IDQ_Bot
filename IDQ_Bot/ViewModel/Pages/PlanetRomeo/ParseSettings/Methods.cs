using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel
    {
        private void InitParseSettings()
        {
            FromCountParsePage = 1;
            ToCountParsePage = 20;
            MinTimeOutOfSteps = 300;
            MaxTimeOutOfSteps = 900;
        }
    }
}
