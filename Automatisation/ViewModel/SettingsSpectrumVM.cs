/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Automatisation.Model;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;

namespace Automatisation.ViewModel
{
    class SettingsSpectrumVM : ObservableObject
    {
        #region fields
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private static string _logTarget = Automatisation.Logging.TargetNames.SPECTRUM;
        private SpectrumMeterModel _spectrumMeter = null; //only used locally for communication: getting and setting analyzer settings
        private SettingsSpectrumModel _spectrumSettings; //only used for binding
        private RelayCommand _viewLoaded; //only used for binding
        #endregion fields

        #region properties
        //only used for binding
        public SettingsSpectrumModel Spectrum { get { return _spectrumSettings; } set { if (value != _spectrumSettings) { _spectrumSettings = value; OnPropertyChanged("Spectrum"); } } }
        //only used for binding: if relaycommand is null, make a new one
        public RelayCommand ViewLoaded { get { return _viewLoaded ?? (_viewLoaded = new RelayCommand(WindowLoaded)); } }
        #endregion properties

         #region constructor
        /// <summary>
        /// Initializes a new instance of the SettingsSpectrumViewModel class.
        /// </summary>
        public SettingsSpectrumVM(/*SpectrumMeterModel s*/)
        {
            //_spectrumMeter = s;
            //this.Spectrum = new SettingsSpectrumModel(s);
        }
        #endregion constructor


        #region events
        private void WindowLoaded()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            _spectrumMeter = appvm.TabSpectrum.Spectrum;

            if (_spectrumMeter != null)
            {
                this.Spectrum = new SettingsSpectrumModel(_spectrumMeter);
                _log.Debug(_logTarget + "Loaded");

                if (Spectrum != null)
                    Spectrum.FillInSettings();
                _spectrumSettings.AllowSend = true; //settings have been set, allow writing new settings
            }
            else
            {
                _log.Info(_logTarget + "No SpectrumAnalyzer found for settingmenu");
                //and close window
                throw new System.NotImplementedException("No SpectrumAnalyzer found for settingmenu");
            }

        }
        #endregion events



    }
}
