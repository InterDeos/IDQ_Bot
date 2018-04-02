using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.ViewModel.Pages.PlanetRomeo
{
    partial class PRViewModel : ViewModelBase
    {
        private int _countParsePage;
        private int _minTimeOutOfSteps;
        private int _maxTimeOutOfSteps;

        public int CountParsePage
        {
            get => _countParsePage;
            set
            {
                _countParsePage = value;
                LabelParsePage = string.Format("Count Parse Page = {0}", _countParsePage);
                RaisePropertiesChanged("CountParsePage", "LabelParsePage");
            }
        }
        public int MinTimeOutOfSteps
        {
            get => _minTimeOutOfSteps;
            set
            {
                _minTimeOutOfSteps = value;
                if (MinTimeOutOfSteps > MaxTimeOutOfSteps) MaxTimeOutOfSteps = MinTimeOutOfSteps;
                LabelMinTimeOutOfSteps = string.Format("Minimal Timeout Steps = {0}", MinTimeOutOfSteps);
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
                LabelMaxTimeOutOfSteps = string.Format("Maximal Timeout Steps = {0}", MaxTimeOutOfSteps);
                RaisePropertiesChanged("MaxTimeOutOfSteps", "LabelMaxTimeOutOfSteps");
            }
        }

        public string LabelParsePage { get; set; }
        public string LabelMinTimeOutOfSteps { get; set; }
        public string LabelMaxTimeOutOfSteps { get; set; }


        private void InitParseSettings()
        {
            CountParsePage = 20;
            MinTimeOutOfSteps = 300;
            MaxTimeOutOfSteps = 900;
        }
    }
}
