/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Ivi.Visa.Interop;
using NLog;
using System;

namespace Automatisation.Model
{
    /// <summary>
    /// This class contains general functions and properties for both the spectrum and power meter.
    /// </summary>
   public class MetersModel : BaseModel
    {
        #region fields
        protected static readonly Logger _log = LogManager.GetCurrentClassLogger();

        //General objects to set up a connection.
        protected ResourceManager _rMngr;
        protected FormattedIO488 _dev;

        //General connection settings
        private string _ip = String.Empty;
        private string _visaAddress = String.Empty;
        private int _timeOut = 120000;

        // Vendor, model and firmware strings, automaticly generated.
        private string _model = String.Empty;
        private string _vendor = String.Empty;
        private string _firmware = String.Empty;
        #endregion fields

        #region properties
        public string Ip {get { return _ip; }set { if (_ip != value) { _ip = value; VisaAddress = "TCPIP::" + _ip + "::INSTR"; OnPropertyChanged("Ip"); } }}
        /// <summary>
        /// Gets en sets VISA address.
        /// </summary>
        /// <example>TCPIP::192.168.1.200::INSTR</example>
        public string VisaAddress { get { return _visaAddress; } set { if (_visaAddress != value) { _visaAddress = value; OnPropertyChanged("VisaAddress"); } } }
        public int TimeOut {get { return _timeOut; }set { if (_timeOut != value) { _timeOut = value; OnPropertyChanged("TimeOut"); } }}
        public string Model{get { return _model; }set { if (_model != value) { _model = value; OnPropertyChanged("Model"); } }}
        public string Vendor{get { return _vendor; }set { if (_vendor != value) { _vendor = value; OnPropertyChanged("Vendor"); } } }
        public string Firmware { get { return _firmware; } set { if (_firmware != value) { _firmware = value; OnPropertyChanged("Firmware"); } } }
        #endregion properties

        #region constructor
        /// <summary>
        /// New instance without arguments. Doesn't fill up any parameters or doesn't start any connection.
        /// </summary>
        public MetersModel()
        {
            _rMngr = new ResourceManager();
            _dev = new FormattedIO488();
        }
        /// <summary>
        /// New instance of MetersModel with parameters. Fills in any needed property to start a connection with the device.
        /// </summary>
        /// <param name="visaOrIP">IP or VISA of the machine. If isIP = true, fill in an ip, else a VISA address.</param>
        /// <param name="timeout">Timeout time for the device in miliseconds.</param>
        /// <param name="isIP">Checks if an IP or VISA address has been filled in.</param>
        /// <example>new MetersModel("TCPIP::192.168.1.200::INSTR", 120000, false) </example>
        public MetersModel(string ip, int timeout)
        {
            _rMngr = new ResourceManager();
            _dev = new FormattedIO488();
            Ip = ip;
            TimeOut = timeout;
        }
        #endregion constructor

        #region events
        #endregion events

        /// <summary>
        /// Search the network to detect every VISA machine currently turned on.
        /// </summary>
        /// <returns>Detected machines with their visa address.</returns>
        public static String[] GetVisaAddresses()
        {
            ResourceManager rm = new ResourceManager();
            if (rm == null) { rm = new ResourceManager(); }
            string[] devices = rm.FindRsrc("?*INSTR");
            return devices;
        }


        /// <summary>
        /// Save general device info to properties
        /// </summary>
        private void SetMeterProperties()
        {
            string idn = string.Empty;
            try
            {
                idn = GetIDN();
            }
            catch (Exception)
            {
                //throw new Exception("GetIDN failed: " + e.ToString());
            }

            if (idn != string.Empty)
            {
                string[] parts = idn.Split(',');
                this.Vendor = parts[0];
                this.Model = parts[1];
                //this.SN = parts[2];
                this.Firmware = parts[3];
            }
        }

        #region MeterFuncties
        /// <summary>
        /// Start a new connection with the machine.
        /// </summary>
        /// <exception>Without a legit address, no connection is possible.</exception>
        public virtual void StartConnection()
        {
            if (this.VisaAddress != String.Empty)
            {
                try
                {
                    _dev.IO = (IMessage)_rMngr.Open(this.VisaAddress, AccessMode.NO_LOCK, this.TimeOut);
                    _dev.IO.Timeout = this.TimeOut;
                    SetMeterProperties();
                }
                catch (Exception) { throw; /*new Exception("Start connection error" + e.Message,e);*/ }
            }
            else
            {
                throw new Exception("Startconnection error: No VISAaddress");
            }
        }
        /// <summary>
        /// Closes the connection with the currently connected device.
        /// </summary>
        public void CloseConnestion()
        {
            if (_dev.IO != null)
            {
                try
                {
                    _dev.IO.Close();
                }
                catch (Exception)
                {
                    throw; /*new Exception("Connection failed closing: " + e.Message,e);*/
                }
            }
        }
        /// <summary>
        /// Send a command, you expect no return value. Check the programming guides.
        /// </summary>
        /// <param name="cmd">SCPI command to execute.</param>
        public void WriteCommand(string cmd)
        {
            try
            {
                _dev.WriteString(cmd, true);
            }
            catch (Exception e)
            {
                throw new Exception("Command: " + cmd + " failed. Error: " + e.Message,e);
            }
        }
        /// <summary>
        /// Send a command, you expect a return value. Check the programming guides..
        /// </summary>
        /// <param name="cmd">SCPI command to execute.</param>
        /// <returns>Result from the executed command.</returns>
        public string WriteCommandRes(string cmd)
        {
            string res = String.Empty;
            //try to execute the command and get a result.
            try
            {
                _dev.WriteString(cmd, true);
                res = _dev.ReadString();
                res = res.Trim('\n');
                //if there is a result, check if it contains a scientific notation and convert it to a prefix
                if (res.ToUpper().Contains("E+"))
                {
                    try
                    {
                        res = Automatisation.Convertors.SientificConvertor.FromSIStringToPrefix(res);
                    }
                    catch (Exception e)
                    {
                        _log.Error("Couldn't convert " + res.ToUpper() + " to Prefix: " + e.Message, e);
                        //throw new Exception("Couldn't convert " + res.ToUpper() + " to Prefix: " + e.Message,e);
                    }
                }
            }
            catch (Exception e)
            {
                res = "Command: " + cmd + " failed: " + e.ToString();
                throw new Exception("Command: " + cmd + " failed: " + e.Message,e);
            }
            return res;
        }


        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public string[] ReadIEEEBlock(string cmd)
        {
            string[] res = null;

            try
            {
               object wfm = _dev.ReadIEEEBlock(IEEEBinaryType.BinaryType_R4);
               byte[] res2 = ObjectToByteArray(wfm);
            }
            catch (Exception)
            {
                throw;
            }

            return res;
        }
        /// <summary>
        /// Gets the general device info.
        /// </summary>
        /// <returns>String in format of "vendor, model, sn, firmware"</returns>
        public string GetIDN()
        {
            string idn = WriteCommandRes(Commands.SCPI.IDN);
            return idn;
        }
        #endregion MeterFuncties
    }
}