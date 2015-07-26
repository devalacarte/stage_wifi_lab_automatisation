/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Automatisation.Logging.Targets;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Automatisation.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public static Logger LOG = LogManager.GetCurrentClassLogger();
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        public ObservableCollection<string> IncomingMessages { get { return _messages; } private set { if (value != _messages) { _messages = value; OnPropertyChanged("IncomingMessages"); } } }
        //private RelayCommand _viewLoaded;
        //public RelayCommand ViewLoaded { get { return _viewLoaded ?? (_viewLoaded = new RelayCommand(Initialized)); } }
        private MainMenuVM _mainMenu;
        public MainMenuVM MainMenu
        {
            get { return _mainMenu; }
            set { if (_mainMenu != value) { _mainMenu = value; OnPropertyChanged("MainMenu"); } }
        }

        private MainUCVM _tabMain;
        public MainUCVM TabMain
        {
            get { return _tabMain; }
            set { if (_tabMain != value) { _tabMain = value; OnPropertyChanged("TabMain"); } }
        }
        
        private PowerVM _tabPwr;
        public PowerVM TabPwr
        {
            get { return _tabPwr; }
            set { if (_tabPwr != value) { _tabPwr = value; OnPropertyChanged("TabPwr"); } }
        }

        private SpectrumVM _tabSpectrum;
        public SpectrumVM TabSpectrum
        {
            get { return _tabSpectrum; }
            set { if (_tabSpectrum != value) { _tabSpectrum = value; OnPropertyChanged("TabSpectrum"); } }
        }

        private AttenuatorsVM _tabAtt;
        public AttenuatorsVM TabAtt
        {
            get { return _tabAtt; }
            set { if (_tabAtt != value) { _tabAtt = value; OnPropertyChanged("TabAtt"); } }
        }

        private IperfVM _tabIperf;
        public IperfVM TabIperf
        {
            get { return _tabIperf; }
            set { if (_tabIperf != value) { _tabIperf = value; OnPropertyChanged("TabIperf"); } }
        }

        private static object _lock = new object();
        public ApplicationVM()
        {
            //Pages.Add(new PageOneVM());
            // Add other pages


            //CurrentPage = Pages[0];
            //CurrentPage = _tabAtt;
            BindingOperations.EnableCollectionSynchronization(_messages, _lock);
            Logging.Config.SetupConfig();
            SetupLogging();

            MainMenu = new MainMenuVM();
            TabMain = new MainUCVM();
            TabAtt = new AttenuatorsVM();
            TabPwr = new PowerVM();
            TabSpectrum = new SpectrumVM();
            TabIperf = new IperfVM();
            LOG.Debug("MainVM: Loaded");
            LOG.Debug("MainVM: Checking if IPs exist");

            IsIpSet();
        }

        private object currentPage;
        public object CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private List<IPage> pages;
        public List<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new List<IPage>();
                return pages;
            }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        private void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        private bool _isIperfEnabled = false;
        public bool IsIperfEnabled
        {
            get { return _isIperfEnabled; }
            set { if (_isIperfEnabled != value) { _isIperfEnabled = value; OnPropertyChanged("IsIperfEnabled"); if (value == true) { TabIperf.EnableIperf(); } else { TabIperf.DisableIperf(); } } }
        }

        private bool _isAttEnabled = false;
        public bool IsAttEnabled
        {
            get { return _isAttEnabled; }
            set { if (_isAttEnabled != value) { _isAttEnabled = value; OnPropertyChanged("IsAttEnabled"); if (value == true) { TabAtt.EnableAttenuators(); } else { TabAtt.DisableAttenuators(); } } }
        }

        private bool _isPowerEnabled = false;
        public bool IsPowerEnabled
        {
            get { return _isPowerEnabled; }
            set { if (_isPowerEnabled != value) { _isPowerEnabled = value; OnPropertyChanged("IsPowerEnabled"); if (value == true) { TabPwr.EnablePowerMeter(); } else { TabPwr.DisablePowerMeter(); } } }
        }

        private bool _isSpectrumEnabled = false;
        public bool IsSpectrumEnabled
        {
            get { return _isSpectrumEnabled; }
            set { if (_isSpectrumEnabled != value) { _isSpectrumEnabled = value; OnPropertyChanged("IsSpectrumEnabled"); if (value == true) { TabSpectrum.EnableSpectrumMeter(); } else { TabSpectrum.DisableSpectrumMeter(); } } }
        }

        /// <summary>
        /// setup target listener, listen to every single logtarget, add messages to a list
        /// </summary>
        private void SetupLogging()
        {
            var target = Logging.Config.GetTarget(Logging.TargetNames.ALL) as TargetAllLogs;
            target.Messages.Subscribe(msg => _messages.Add(msg));
            IncomingMessages = _messages;
        }

        /// <summary>
        /// Check if IPs for every device is set, if not launch the IP setting window
        /// </summary>
        private void IsIpSet()
        {
            bool checkIP = false;
            try
            {
                checkIP = SettingsIPVM.SettingsSetCheck();
            }
            catch (Exception e)
            {
                LOG.Error("MainVM: Error while Checking IPs", e);
            }
            finally
            {
                if (checkIP == true)
                {
                    LOG.Debug("MainVM: IPs are set");
                }
                else
                {
                    SettingsIPShow();
                }
            }
        }

        /// <summary>
        ///Send notification to MainView to launch a new Settings IP Window
        /// </summary>
        public static void SettingsIPShow()
        {
            LOG.Debug("MainVM: Launching IP settings");
            Messenger.Default.Send(new NotificationMessage(Messages.Messages.SETTINGSIP_LAUNCH));
        }

        /// <summary>
        ///Send notification to MainView to launch a new Settings Spectrum Window
        /// </summary>
        public static void SettingsSpectrumShow()
        {
            LOG.Debug("MainVM: Launching Spectrum settings");
            Messenger.Default.Send(new NotificationMessage(Messages.Messages.SETTINGSSPECTRUM_LAUNCH));
        }
    }
}
