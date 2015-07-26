using Automatisation.Logging.Targets;
using Automatisation.Networking;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatisation.ViewModel
{
    class IperfVM : ObservableObject
    {
        #region fields
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private static string _logTarget = Automatisation.Logging.TargetNames.IPERF;
        private ObservableCollection<string> _messages;
        private Boolean _isLogSet = false;

        private ObservableCollection<SSHVM> _lstSSHClients;
        //private ObservableCollection<WalburySoftware.TerminalControl> _lstSSHClients;
        //private RelayCommand _startConnection;
        //private RelayCommand _executeCommand;
        #endregion fields

        #region properties
        public ObservableCollection<string> IncomingMessages { get { return _messages; } private set { if (value != _messages) { _messages = value; OnPropertyChanged("IncomingMessages"); } } }
        public ObservableCollection<SSHVM> LstSSHClients { get { return _lstSSHClients; } private set { if (value != _lstSSHClients) { _lstSSHClients = value; OnPropertyChanged("LstSSHClients"); } } }
        //public ObservableCollection<WalburySoftware.TerminalControl> LstSSHClients { get { return _lstSSHClients; } private set { if (value != _lstSSHClients) { _lstSSHClients = value; OnPropertyChanged("LstSSHClients"); } } }
        //public RelayCommand ConnectStart { get { return _executeCommand ?? (_executeCommand = new RelayCommand(ConnectClients)); } }

        #endregion properties

        #region constructor
        /// <summary>
        /// Initializes a new instance of the TabIPERFViewModel class.
        /// </summary>
        public IperfVM()
        {
        }
        #endregion constructor

        public void EnableIperf()
        {
            if (_isLogSet == false)
                SetupLogging();
            _log.Debug(_logTarget +"EnabledIperf");
            //CreateSSHClients();
        }

        /// <summary>
        /// Disable the powermeter and current logs for this meter. Delete any current results
        /// </summary>
        public void DisableIperf()
        {
            _log.Debug(_logTarget +"Disablediperf");
            //CloseSSHClients();
            //IncomingMessages.Clear();
        }

        /// <summary>
        /// setup target listener, listen to logs for the powermeter, add messages to a list
        /// </summary>
        private void SetupLogging()
        {
            //setup target listener
            var target = Logging.Config.GetTarget(Logging.TargetNames.IPERF) as TargetIperfLogs;
            target.Messages.Subscribe(msg => _messages.Add(msg));
            IncomingMessages = _messages;
            _isLogSet = true;
        } 
 
        /// <summary>
        /// Create a new powermeter.
        /// Check if the powermeter has a valid IP. If no IP is set, launch the settings menu, else start the connection.
        /// Once the connection is available check if the sensors are connected
        /// </summary>
        public void CreateSSHClients()
        {
            IncomingMessages = new ObservableCollection<string>();
            LstSSHClients = new ObservableCollection<SSHVM>();
            //LstSSHClients = new ObservableCollection<WalburySoftware.TerminalControl>();
            if (String.IsNullOrEmpty(Properties.Settings.Default.IP_IPERF_CLIENT) || String.IsNullOrEmpty(Properties.Settings.Default.IPERFCLIENTUSER) || String.IsNullOrEmpty(Properties.Settings.Default.IPERFCLIENTPASS) || String.IsNullOrEmpty(Properties.Settings.Default.IP_IPERF_SERVER) || String.IsNullOrEmpty(Properties.Settings.Default.IPERFSERVERUSER) || String.IsNullOrEmpty(Properties.Settings.Default.IPERFSERVERPASS))
            {
                _log.Debug(_logTarget + "Settings for SSH not set!");
                Messenger.Default.Send(new NotificationMessage(Messages.Messages.SETTINGSIP_LAUNCH));
            }
            else
            {
                
                /*WalburySoftware.TerminalControl t1 = new WalburySoftware.TerminalControl();
                t1.Host = "192.168.17.128"; t1.Password = "luna1991"; t1.UserName = "root"; t1.Method = WalburySoftware.ConnectionMethod.SSH2;
                WalburySoftware.TerminalControl t2 = new WalburySoftware.TerminalControl("root", "luna1991", "192.168.17.129", WalburySoftware.ConnectionMethod.SSH2);
                LstSSHClients.Add(t1);
                LstSSHClients.Add(t2);*/
                //ConnectClients();
                //t1.SetPaneColors(System.Drawing.Color.White, System.Drawing.Color.Black);
                //t2.SetPaneColors(System.Drawing.Color.White, System.Drawing.Color.Black);
                
                SSHVM s;
                s= new SSHVM(Properties.Settings.Default.IP_IPERF_CLIENT, Properties.Settings.Default.IPERFCLIENTUSER, Properties.Settings.Default.IPERFCLIENTPASS, Properties.Settings.Default.IPERFCLIENTPORT);
                s.ID = 1;
                SSHVM s2;
                s2= new SSHVM(Properties.Settings.Default.IP_IPERF_SERVER, Properties.Settings.Default.IPERFSERVERUSER, Properties.Settings.Default.IPERFSERVERPASS, Properties.Settings.Default.IPERFSERVERPORT);
                s.ID = 2;

                LstSSHClients.Add(s);
                LstSSHClients.Add(s2);
            }
        }

        /*private void ConnectClients()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    foreach (WalburySoftware.TerminalControl item in LstSSHClients)
                    {
                        item.Connect();
                    }
                }
            );
        }*/


        /*
        public void CloseSSHClients()
        {
            foreach (SSHVM item in LstSSHClients)
            {
                item.CloseConnection();
                LstSSHClients.Remove(item);
            }
        }*/

        /*private SSHVM GetSSHBClientyID(int id)
        {
            foreach (SSHVM client in LstSSHClients)
            {
                if (client.ID == id)
                    return client;
            }
            return null;
        }*/
    }
}
