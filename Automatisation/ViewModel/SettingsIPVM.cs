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
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Windows;

namespace Automatisation.ViewModel
{
    
    class SettingsIPVM : INotifyPropertyChanged
    {
        #region fields
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private SettingsIPModel _IPs;
        private string _logFolder = "Not set";
        public static string IP_NOT_VALID = "Not a valid ip!";

        private RelayCommand<string> _cmdPing;
        private RelayCommand _menuFileSave;
        private RelayCommand<CancelEventArgs> _menuFileExit;
        private RelayCommand<CancelEventArgs> _viewClosing;
        private RelayCommand _viewLoaded;
        private RelayCommand _cmdFolderLocation;
        #endregion fields

        #region properties
        public SettingsIPModel IPs { get { return _IPs; } set { if (value != _IPs) { _IPs = value; OnPropertyChanged("IPs"); } } }
        public string LogFolder { get { return _logFolder; } set { if (value != _logFolder) { _logFolder = value; OnPropertyChanged("LogFolder"); IPs.IsChanged = true; } } }

        public RelayCommand<string> Ping { get { return _cmdPing ?? (_cmdPing = new RelayCommand<string>(s => PingIP(s))); } } //2x ?? checks if _ping == null
        public RelayCommand MenuFileSave { get { return _menuFileSave ?? (_menuFileSave = new RelayCommand(SettingsSave)); } }
        public RelayCommand<CancelEventArgs> MenuFileExit { get { return _menuFileExit ?? (_menuFileExit = new RelayCommand<CancelEventArgs>((e) => ShowMessageBoxExit(e))); } }
        public RelayCommand<CancelEventArgs> ViewClosing { get { return _viewClosing ?? (_viewClosing = new RelayCommand<CancelEventArgs>((e) => ShowMessageBoxExit(e))); } }
        public RelayCommand ViewLoaded { get { return _viewLoaded ?? (_viewLoaded = new RelayCommand(SettingsLoad)); } }
        public RelayCommand CMDFolderLocation { get { return _cmdFolderLocation ?? (_cmdFolderLocation = new RelayCommand(ChooseFolderLocation)); } }
        #endregion properties


        public SettingsIPVM() 
        {
            
        }


        private void SettingsLoad()
        {
            IPs = new SettingsIPModel();
            SettingsGet();
            _log.Debug("SettingsIPVM: Loaded");
        }

        private void SettingsGet()
        {
            IPs.IPDatabase = Properties.Settings.Default.IP_DATABASE;
            IPs.IPFileserver = Properties.Settings.Default.IP_FILESERVER;
            
            IPs.IPIperfClient = Properties.Settings.Default.IP_IPERF_CLIENT;
            IPs.IPERFClientUser = Properties.Settings.Default.IPERFCLIENTUSER;
            IPs.IPERFClientPass = Properties.Settings.Default.IPERFSERVERPASS;
            IPs.IPERFClientPort = Properties.Settings.Default.IPERFCLIENTPORT;

            IPs.IPIperfServer = Properties.Settings.Default.IP_IPERF_SERVER;
            IPs.IPERFServerUser = Properties.Settings.Default.IPERFSERVERUSER;
            IPs.IPERFServerPass = Properties.Settings.Default.IPERFSERVERPASS;
            IPs.IPERFServerPort = Properties.Settings.Default.IPERFSERVERPORT;
            
            IPs.IPPower = Properties.Settings.Default.IP_AGILENT;
            IPs.IPSpectrum = Properties.Settings.Default.IP_TEKTRONIX;

            LogFolder = Properties.Settings.Default.NLOG_LOGFILE_LOCATION;
            IPs.IsChanged = false;
        }

        private void SettingsSave()
        {
            Properties.Settings.Default.IP_DATABASE = IPs.IPDatabase;
            Properties.Settings.Default.IP_FILESERVER = IPs.IPFileserver;
            
            Properties.Settings.Default.IP_IPERF_CLIENT = IPs.IPIperfClient;
            Properties.Settings.Default.IPERFCLIENTUSER = IPs.IPERFClientUser;
            Properties.Settings.Default.IPERFSERVERPASS = IPs.IPERFClientPass;
            Properties.Settings.Default.IPERFCLIENTPORT = IPs.IPERFClientPort;
            
            Properties.Settings.Default.IP_IPERF_SERVER = IPs.IPIperfServer;
            Properties.Settings.Default.IPERFSERVERUSER = IPs.IPERFServerUser;
            Properties.Settings.Default.IPERFSERVERPASS = IPs.IPERFServerPass;
            Properties.Settings.Default.IPERFSERVERPORT = IPs.IPERFServerPort;
            
            Properties.Settings.Default.IP_AGILENT = IPs.IPPower;
            Properties.Settings.Default.IP_TEKTRONIX = IPs.IPSpectrum;

            Properties.Settings.Default.NLOG_LOGFILE_LOCATION = LogFolder;
            Properties.Settings.Default.Save();
            IPs.IsChanged = false;
            _log.Debug("SettingsIPVM: Settings Saved");
        }

        private string IPSetting(string ip)
        {
            return (Networking.IP.checkValidIP(ip) == true) ? ip : IP_NOT_VALID;
        }

        /// <summary>
        /// Checks if every single IP in the settings have been set correctly. Throws a custom exception if not.
        /// </summary>
        /// <returns>True if all IP settings have been set correctly.</returns>
        public static bool SettingsSetCheck()
        {
            bool result = true;
            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_AGILENT)){ result = false; throw new Exception("IP not set: Agilent"); }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_TEKTRONIX)) { result = false; throw new Exception("IP not set: Tektronix"); }

            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_DATABASE)) { result = false; throw new Exception("IP not set: Database"); }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_FILESERVER)) { result = false; throw new Exception("IP not set: Fileserver"); }
            
            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_IPERF_CLIENT)) { result = false; throw new Exception("IP not set: Iperf Client"); }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IPERFCLIENTUSER)) { result = false; throw new Exception("User not set: Iperf Client"); }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IPERFCLIENTPASS)) { result = false; throw new Exception("Pass not set: Iperf Client"); }
            if (Properties.Settings.Default.IPERFCLIENTPORT==0) { result = false; throw new Exception("Port not set: Iperf Client"); }

            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_IPERF_SERVER)){ result = false; throw new Exception("IP not set: Iperf Server"); }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IPERFSERVERUSER)) { result = false; throw new Exception("User not set: Iperf Server"); }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IPERFSERVERPASS)) { result = false; throw new Exception("Pass not set: Iperf Server"); }
            if (Properties.Settings.Default.IPERFSERVERPORT == 0) { result = false; throw new Exception("Port not set: Iperf Server"); }

            if (String.IsNullOrEmpty(Properties.Settings.Default.NLOG_LOGFILE_LOCATION)) { result = false; throw new Exception("Logfolder location not set"); }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        private void PingIP(string ip)
        {
            string ping = Networking.IP.PingIP(ip);
            _log.Info(ping);
            NotificationMessage<string> nm = new NotificationMessage<string>(ping, Messages.Messages.SETTINGSIP_PING);
            Messenger.Default.Send(nm);
        }

        private void ShowMessageBoxExit(CancelEventArgs e)
        {
            if(_IPs.IsChanged == true)
            {
            var msg = new NotificationMessageAction<MessageBoxResult>(this, Messages.Messages.SETTINGSIP_SAVE, (r) =>
            {
                if (r == MessageBoxResult.Yes)
                {
                    // do stuff
                    e.Cancel = true;
                    SettingsSave();
                }
            });
            _log.Debug("Sending message to View to SaveSettingsIp Dialog");
            Messenger.Default.Send(msg);
            }
        }

        private void ChooseFolderLocation()
        {
            Messenger.Default.Send(new Messages.SelectFolderMessage((pathfromview) => { LogFolder = pathfromview; }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (propertyName == "IPs")
                {
                    IPs.IsChanged = true;
                }
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
