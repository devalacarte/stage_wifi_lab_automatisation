/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Renci.SshNet;

namespace Automatisation.Model
{
    public class SSH : BaseModel
    {
        #region fields
        private int _id;
        private string _description;
        private string _host;
        private string _user;
        private string _password;
        private int _port = 22;
        private SshClient _clientSSH;
        #endregion fields


        #region properties
        public int ID { get { return _id; } set { if (value != _id) { _id = value; OnPropertyChanged("ID"); } } }
        public string Description { get { return _description; } set { if (value != _description) { _description = value; OnPropertyChanged("Description"); } } }
        public string HostName { get { return _host; } set { if (value != _host) { _host = value; OnPropertyChanged("HostName"); } } }
        public string UserName { get { return _user; } set { if (value != _user) { _user = value; OnPropertyChanged("UserName"); } } }
        public string Password { get { return _password; } set { if (value != _password) { _password = value; OnPropertyChanged("Password"); } } }
        public int Port { get { return _port; } set { if (value != _port) { _port = value; OnPropertyChanged("Port"); } } }
        public SshClient ClientSSH { get { return _clientSSH; } set { if (value != _clientSSH) { _clientSSH = value; OnPropertyChanged("ClientSSH"); } } }
        #endregion properties



        #region constructor
        public SSH() { }
        public SSH(string host)
        {
            this.HostName = host;
        }

        public SSH(string host, string user, string pass, int port = 22)
        {
            this.HostName = host;
            this.UserName = user;
            this.Password = pass;
            this.Port = port;
        }
        #endregion constructor
    }
}
