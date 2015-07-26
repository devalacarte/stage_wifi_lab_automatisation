using Automatisation.Model;
/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using NLog;
using Renci.SshNet;
using System;
using System.Collections.ObjectModel;

namespace Automatisation.ViewModel
{
    class SSHVM : ViewModelBase
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        #region fields
        private SSH _ssh;
        private string _cmd;
        private RelayCommand _viewLoaded;
        private RelayCommand _executeCommand;
        private ObservableCollection<string> _results;
        //private bool _canExecute = true;
        #endregion fields

        #region properties
        private int _id;
        public int ID { get { return _id; } set { if (value != _id) { _id = value; RaisePropertyChanged("ID"); } } }

        public SSH Ssh
        {
            get { return _ssh; }
            set { if (_ssh != value) { _ssh = value; RaisePropertyChanged("Ssh"); } }
        }
        public string CommandToExecute
        {
            get { return _cmd; }
            set { if (_cmd != value) { _cmd = value; RaisePropertyChanged("CommandToExecute"); } }
        }
        public RelayCommand ViewLoaded { get { return _viewLoaded ?? (_viewLoaded = new RelayCommand(WindowLoaded)); } }
        public RelayCommand ExecuteCommand { get { return _executeCommand ?? (_executeCommand = new RelayCommand(ExecuteTwo)); } }
        public ObservableCollection<string> Results
        {
            get { return _results; }
            set { if (_results != value) { _results = value; RaisePropertyChanged("Results"); } }
        }
        #endregion properties

        #region constructor
        /// <summary>
        /// Initializes a new instance of the SSHViewModel class.
        /// </summary>
        public SSHVM(string ip, string username, string password, int port)
        {
            //Messenger.Default.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));
            /*try
            {
                if (Results == null)
                    Results = new ObservableCollection<string>();
                Ssh = new SSH(ip, username, password, port);
                CreateSSHClient();
                StartConnection();
            }
            catch (System.Exception)
            {
                return;
            }*/
        }
        #endregion constructor

        public void CreateSSHClient()
        {
            if (String.IsNullOrEmpty(Ssh.HostName) || String.IsNullOrEmpty(Ssh.UserName) || String.IsNullOrEmpty(Ssh.Password))
                return;

            if (Networking.IP.checkValidIP(Ssh.HostName) == false) { throw new Exception(Ssh.HostName + "is not a valid IP address"); }

            PasswordConnectionInfo cinfo = new PasswordConnectionInfo(Ssh.HostName, Ssh.Port, Ssh.UserName, Ssh.Password);
            Ssh.ClientSSH = new SshClient(cinfo);
        }

        public void StartConnection()
        {
            if (Ssh.ClientSSH == null)
                return;

            if ((Ssh.ClientSSH.IsConnected == true))
                Ssh.ClientSSH.Disconnect();

            Ssh.ClientSSH.Connect();
        }

        public void CloseConnection()
        {
            if (Ssh.ClientSSH != null && Ssh.ClientSSH.IsConnected)
                Ssh.ClientSSH.Disconnect();
        }

        /*public string WriteCommand(string command)
        {
            string res = "No result";
            SshCommand cmd = Ssh.ClientSSH.CreateCommand(command);
            System.IAsyncResult ares = cmd.BeginExecute();
            System.IO.StreamReader sr = new System.IO.StreamReader(cmd.OutputStream);
            while (!ares.IsCompleted)
            {
                string s = sr.ReadToEnd();
                if (string.IsNullOrEmpty(s))
                    continue;
                Console.WriteLine(s);
            }
            cmd.EndExecute(ares);
            //Console.WriteLine("SSH Error: {0}",cmd.Error); 
            //Console.WriteLine("SSH Error: {0}",cmd.Result);
            //Console.WriteLine("SSH Error: {0}", cmd.ExitStatus);
            //Console.WriteLine("SSH Error: {0}", cmd.OutputStream);
            return res;
        }*/

        private void ExecuteTwo()
        {
            if (Ssh == null)
                return;

            if (string.IsNullOrEmpty(_cmd))
                return;

            System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    //DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Info(_logTarget + "Creating new PowerMeter"); });
                    Renci.SshNet.SshCommand cmd = Ssh.ClientSSH.CreateCommand(_cmd);
                    System.IAsyncResult ares = cmd.BeginExecute();
                    System.IO.StreamReader sr = new System.IO.StreamReader(cmd.OutputStream);
                    while (!ares.IsCompleted)
                    {
                        string s = sr.ReadToEnd();
                        if (string.IsNullOrEmpty(s))
                            continue;
                        DispatcherHelper.CheckBeginInvokeOnUI(() => { Console.WriteLine(this.ID + "------" + s); Results.Add(s); });
                    }
                }
            );
        }

        private void Execute()
        {
            if (Ssh == null)
                return;

            if (string.IsNullOrEmpty(_cmd))
                return;

            Renci.SshNet.SshCommand cmd = Ssh.ClientSSH.CreateCommand(_cmd);
            System.IAsyncResult ares = cmd.BeginExecute();
            System.IO.StreamReader sr = new System.IO.StreamReader(cmd.OutputStream);
            while (!ares.IsCompleted)
            {
                string s = sr.ReadToEnd();
                if (string.IsNullOrEmpty(s))
                    continue;
                Console.WriteLine(s);
                Results.Add(s);
            }
        }

        private void WindowLoaded()
        { 
        
        }
    }
}
