/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Automatisation.Model.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Automatisation.Model
{
    public class SettingsIPModel : ValidationBase
    {
        #region fields
        private bool _isChanged = false;
        /// <summary>
        /// 
        /// </summary>
        private string _ipPower;

        /// <summary>
        /// 
        /// </summary>
        private string _ipSpectrum;

        /// <summary>
        /// 
        /// </summary>
        private string _ipFileServer;

        /// <summary>
        /// 
        /// </summary>
        private string _ipDB;

        /// <summary>
        /// 
        /// </summary>
        private string _ipIperfClient;
        /// <summary>
        /// 
        /// </summary>
        private string _iperfClientUser;
        /// <summary>
        /// 
        /// </summary>
        private string _iperfClientPass;

        /// <summary>
        /// 
        /// </summary>
        private int _iperfClientPort;

        /// <summary>
        /// 
        /// </summary>
        private string _ipIperfServer;

        /// <summary>
        /// 
        /// </summary>
        private string _iperfServerUser;

        /// <summary>
        /// 
        /// </summary>
        private string _iperfServerPass;

        /// <summary>
        /// 
        /// </summary>
        private int _iperfServerPort;

        #endregion fields


        public const string IPv4ADDRESS = @"^([0-9]{1,3}[.]{1}[0-9]{1,3}[.]{1}[0-9]{1,3}[.]{1}[0-9]{1,3})$";
        #region properties
        public Boolean IsChanged { get { return _isChanged; } set { if (value != _isChanged) { _isChanged = value; } } }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [RegularExpression(IPv4ADDRESS, ErrorMessage = "Not a valid IP")]
        public string IPPower
        {
            get { return _ipPower; }
            set { if (_ipPower != value) { _ipPower = value; OnPropertyChanged("IPPower"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [RegularExpression(IPv4ADDRESS, ErrorMessage = "Not a valid IP")]
        public string IPSpectrum
        {
            get { return _ipSpectrum; }
            set { if (_ipSpectrum != value) { _ipSpectrum = value; OnPropertyChanged("IPSpectrum"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [RegularExpression(IPv4ADDRESS, ErrorMessage = "Not a valid IP")]
        public string IPFileserver
        {
            get { return _ipFileServer; }
            set { if (_ipFileServer != value) { _ipFileServer = value; OnPropertyChanged("IPFileserver"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [RegularExpression(IPv4ADDRESS, ErrorMessage = "Not a valid IP")]
        public string IPDatabase
        {
            get { return _ipDB; }
            set { if (IPDatabase != value) { _ipDB = value; OnPropertyChanged("IPDatabase"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [RegularExpression(IPv4ADDRESS, ErrorMessage = "Not a valid IP")]
        public string IPIperfClient
        {
            get { return _ipIperfClient; }
            set { if (_ipIperfClient != value) { _ipIperfClient = value; OnPropertyChanged("IPIperfClient"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string IPERFClientUser
        {
            get { return _iperfClientUser; }
            set { if (_iperfClientUser != value) { _iperfClientUser = value; OnPropertyChanged("IPERFClientUser"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string IPERFClientPass
        {
            get { return _iperfClientPass; }
            set { if (_iperfClientPass != value) { _iperfClientPass = value; OnPropertyChanged("IPERFClientPass"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [Range(0, 64000, ErrorMessage = "port not in range")]
        public int IPERFClientPort
        {
            get { return _iperfClientPort; }
            set { if (_iperfClientPort != value) { _iperfClientPort = value; OnPropertyChanged("IPERFClientPort"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [RegularExpression(IPv4ADDRESS, ErrorMessage = "Not a valid IP")]
        public string IPIperfServer
        {
            get { return _ipIperfServer; }
            set { if (_ipIperfServer != value) { _ipIperfServer = value; OnPropertyChanged("IPIperfServer"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string IPERFServerUser
        {
            get { return _iperfServerUser; }
            set { if (_iperfServerUser != value) { _iperfServerUser = value; OnPropertyChanged("IPERFServerUser"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string IPERFServerPass
        {
            get { return _iperfServerPass; }
            set { if (_iperfServerPass != value) { _iperfServerPass = value; OnPropertyChanged("IPERFServerPass"); ValidateProperty(value); } }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [Range(1, 64000, ErrorMessage = "port not in range")]
        public int IPERFServerPort
        {
            get { return _iperfServerPort; }
            set { if (_iperfServerPort != value) { _iperfServerPort = value; OnPropertyChanged("IPERFServerPort"); ValidateProperty(value); } }
        }
        #endregion properties

        #region constructor
        public SettingsIPModel()
        {

        }
        #endregion constructor

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
            {
                _isChanged = true;
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }
    }
}