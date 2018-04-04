using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel
    {
        public int ToCountParsePage
        {
            get => _toCountParsePage;
            set
            {
                _toCountParsePage = value;
                if (ToCountParsePage < FromCountParsePage) FromCountParsePage = ToCountParsePage;
                LabelParsePage = string.Format("Parse Pages From {0} To {1}", FromCountParsePage, ToCountParsePage);
                RaisePropertiesChanged("FromCountParsePage", "LabelParsePage", "ToCountParsePage");
            }
        }
        public int FromCountParsePage
        {
            get => _fromcountParsePage;
            set
            {
                _fromcountParsePage = value;
                if (FromCountParsePage > ToCountParsePage) ToCountParsePage = FromCountParsePage;
                LabelParsePage = string.Format("Parse Pages From {0} To {1}", FromCountParsePage, ToCountParsePage);
                RaisePropertiesChanged("FromCountParsePage", "LabelParsePage", "ToCountParsePage");
            }
        }
        public int MinTimeOutOfSteps
        {
            get => _minTimeOutOfSteps;
            set
            {
                _minTimeOutOfSteps = value;
                if (MinTimeOutOfSteps > MaxTimeOutOfSteps) MaxTimeOutOfSteps = MinTimeOutOfSteps;
                LabelMinTimeOutOfSteps = string.Format("Minimal Timeout Steps = {0} Maximal = {1}", MinTimeOutOfSteps, MaxTimeOutOfSteps);
                RaisePropertiesChanged("MinTimeOutOfSteps", "LabelMinTimeOutOfSteps");
            }
        }
        public int MaxTimeOutOfSteps
        {
            get => _maxTimeOutOfSteps;
            set
            {
                _maxTimeOutOfSteps = value;
                if (MaxTimeOutOfSteps < MinTimeOutOfSteps) MinTimeOutOfSteps = MaxTimeOutOfSteps;
                LabelMinTimeOutOfSteps = string.Format("Minimal Timeout Steps {0} Maximal {1}", MinTimeOutOfSteps, MaxTimeOutOfSteps);
                RaisePropertiesChanged("MaxTimeOutOfSteps", "LabelMinTimeOutOfSteps");
            }
        }

        public string LabelParsePage { get; set; }
        public string LabelMinTimeOutOfSteps { get; set; }
        public string LabelMaxTimeOutOfSteps { get; set; }
    }
}
