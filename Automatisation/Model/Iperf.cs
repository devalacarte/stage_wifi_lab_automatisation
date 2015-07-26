/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Automatisation.Networking;
namespace Automatisation.Model
{
    public class Iperf : BaseModel
    {

        #region fields
        private SSH _ssh;
        #endregion fields


        #region properties
        public SSH OSSH { get { return _ssh; } set { if (value != _ssh) { _ssh = value; OnPropertyChanged("OSSH"); } } }
        #endregion properties



        #region constructor
        public Iperf() { }
        public Iperf(string host)
        {
            OSSH = new SSH(host);
        }
        public Iperf(string host, string user, string pass)
        {
            OSSH = new SSH(host, user, pass);
        }
        public Iperf(SSH ssh)
        {
            OSSH = ssh;
        }
        #endregion constructor

       
    }
}
