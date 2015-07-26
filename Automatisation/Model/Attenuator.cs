/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using System;
using System.Timers;

namespace Automatisation.Model
{
    public class Attenuator : BaseModel
    {
        #region fields
        private bool _isPowered = false;
        private string _label = "No device";
        private uint _devID = 0;
        private int _serialNumber = 0;
        private string _model;
        private int _deviceStatus;
        private int _attenuation = 0;
        private int _attenuationStep = 2;
        private int _attenuationMin;
        private double _attenuationMinDB;
        private int _attenuationMax;
        private double _attenuationMaxDB;
        private int _rampStart = 0;
        private int _rampEnd = 0;
        private int _dwellTime = 1000;
        private int _idleTime = 0;
        private bool _rf_on = true;
        private bool _isRampModeRepeat = false;
        private bool _isRampDirectionUp = true;


        private Timer _tmrRefreshAttenuation;
        #endregion fields

        #region properties
        /// <summary>
        /// Set to true if the device has been found
        /// </summary>
        public bool IsEnabled { get { return _isPowered; } set { if (value != _isPowered) { _isPowered = value; OnPropertyChanged("IsEnabled"); } } }
        /// <summary>
        /// Labeled according to label numbers on the device for easy recognition
        /// </summary>
        public string Label { get { return _label; } set { if (value != _label) { _label = value; OnPropertyChanged("Label"); } } }
        /// <summary>
        /// Device number being used for every command
        /// </summary>
        public uint DeviceNumber { get { return _devID; } set { if (value != _devID) { _devID = value; OnPropertyChanged("DeviceNumber"); } } }
        public int SerialNumber { get { return _serialNumber; } set { if (value != _serialNumber) { _serialNumber = value; OnPropertyChanged("SerialNumber"); } } }
        public string Model { get { return _model; } set { if (value != _model) { _model = value; OnPropertyChanged("Model"); } } }
        /// <summary>
        /// Used to store flags
        /// </summary>
        public int DeviceStatus { get { return _deviceStatus; } set { if (value != _deviceStatus) { _deviceStatus = value; OnPropertyChanged("DeviceStatus"); } } }
        /// <summary>
        /// Attenuation in db (0.25 * value retrieved from attenuator)
        /// </summary>
        public double Attenuation { 
            get { return _attenuation * 0.25; } 
            set { if ((int)value * 4 != _attenuation) {
                if (CheckMinMaxDB((int)value * 4)) {
                    _attenuation = (int)value * 4;
                    Libs.VNX_Atten.SetAttenuation(_devID, _attenuation); OnPropertyChanged("Attenuation");
                }
            }
            }
        }
        /// <summary>
        /// AttenuationStep in db (0.25 * value retrieved from attenuator)
        /// </summary>
        public double AttenuationStep { get { return _attenuationStep * 0.25; } set { if ((int)value * 4 != _attenuationStep) { _attenuationStep = (int)(value * 4); Libs.VNX_Atten.SetAttenuationStep(_devID,_attenuationStep); OnPropertyChanged("AttenuationStep"); } } }
        /// <summary>
        /// Every attenuator series has a minimum dB value, being 0 for our devices.
        /// </summary>
        public int AttenuationMin { get { return _attenuationMin; } set { if (value != _attenuationMin) { _attenuationMin = value; AttenuationMinDB = value * 0.25; OnPropertyChanged("AttenuationMin"); } } }
        public double AttenuationMinDB { get { return _attenuationMinDB; } set { if (value != _attenuationMinDB) { _attenuationMinDB = value; OnPropertyChanged("AttenuationMinDB"); } } }
        /// <summary>
        /// Every attenuator series has a maximum dB value, being 63 for our devices.
        /// </summary>
        public int AttenuationMax { get { return _attenuationMax; } set { if (value != _attenuationMax) { _attenuationMax = value; AttenuationMaxDB = value * 0.25; OnPropertyChanged("AttenuationMax"); } } }
        public double AttenuationMaxDB { get { return _attenuationMaxDB; } set { if (value != _attenuationMaxDB) { _attenuationMaxDB = value; OnPropertyChanged("AttenuationMaxDB"); } } }

        public double RampStart { 
            get { return _rampStart * 0.25; } 
            set { if ((int)value * 4 != _rampStart) { 
                if (CheckMinMaxDB((int) value*4)) {
                    _rampStart = (int)value * 4;
                    IsSweepUp();
                    Libs.VNX_Atten.SetRampStart(_devID, _rampStart); OnPropertyChanged("RampStart"); } } } }
        /// <summary>
        /// RampEnd in db (0.25 * value retrieved from attenuator)
        /// </summary>
        public double RampEnd { 
            get { return _rampEnd * 0.25; } 
            set { if ((int)value * 4 != _rampEnd) {
                if (CheckMinMaxDB((int) value*4)) {
                    _rampEnd = (int)value * 4;
                    IsSweepUp();
                    Libs.VNX_Atten.SetRampEnd(_devID, _rampEnd); OnPropertyChanged("RampEnd"); } } } }           
        /// <summary>
        /// DwellTime in ms
        /// </summary>
        public int DwellTime { get { return _dwellTime; } set { if (value != _dwellTime) { _dwellTime = value; Libs.VNX_Atten.SetDwellTime(_devID, _dwellTime); OnPropertyChanged("DwellTime"); } } }
        /// <summary>
        /// IdleTime in ms
        /// </summary>
        public int IdleTime { get { return _idleTime; } set { if (value != _idleTime) { _idleTime = value; Libs.VNX_Atten.SetIdleTime(_devID, _idleTime); OnPropertyChanged("IdleTime"); } } }
        
        public bool IsRFOn { get { return _rf_on; } set { if (value != _rf_on) { _rf_on = value; Libs.VNX_Atten.SetRFOn(_devID, _rf_on); OnPropertyChanged("IsRFOn"); } } }
        /// <summary>
        /// Set to off to complete the sweep once. To true to do another sweep after IdleTime has passed.
        /// </summary>
        public bool IsRampModeRepeat { get { return _isRampModeRepeat; } set { if (value != _isRampModeRepeat) { _isRampModeRepeat = value; Libs.VNX_Atten.SetRampMode(_devID, _isRampModeRepeat); OnPropertyChanged("IsRampMode"); } } }
        /// <summary>
        /// Checks if ramp should add or substract attenuationStep. Set up to true when rampEnd > start
        /// </summary>
        public bool IsRampDirectionUp { get { return _isRampDirectionUp; } set { if (value != _isRampDirectionUp) { _isRampDirectionUp = value; Libs.VNX_Atten.SetRampMode(_devID, _isRampDirectionUp); OnPropertyChanged("IsRampDirectionUp"); } } }

        #endregion properties

        #region constructor
        public Attenuator(uint devid)
        {
            this.DeviceNumber = devid;
            GetSettings();
            _tmrRefreshAttenuation = new Timer();
            _tmrRefreshAttenuation.Elapsed += _tmrRefreshAttenuation_Elapsed;
        }
        public Attenuator(uint devid, int serialnumber)
        {
            this.DeviceNumber = devid;
            this.SerialNumber = serialnumber;
            GetSettings();

            _tmrRefreshAttenuation = new Timer();
            _tmrRefreshAttenuation.Elapsed += _tmrRefreshAttenuation_Elapsed;
        }
        #endregion constructor

        #region events
        /// <summary>
        /// Get the new attenuation value after each interval during a sweep/ramp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tmrRefreshAttenuation_Elapsed(object sender, ElapsedEventArgs e)
        {
            int getAtt = Libs.VNX_Atten.GetAttenuation(_devID);
            double getAttDouble = getAtt * 0.25;
            Attenuation = getAttDouble;
            Console.WriteLine("ticked: _att: " + _label + ":" + _devID + " - " +getAtt + " - " + getAttDouble);
        }
        #endregion events

        public void RefreshAttenuation()
        {
            this.Attenuation = Libs.VNX_Atten.GetAttenuation(_devID) * 0.25;
        }

        public void GetSettings()
        {
            if (_devID != 0)
            {
                this.Model = GetModel();
                this.SerialNumber = Libs.VNX_Atten.GetSerialNumber(_devID);
                this.Attenuation = Libs.VNX_Atten.GetAttenuation(_devID)*0.25;
                this.AttenuationMin = Libs.VNX_Atten.GetMinAttenuation(_devID);
                this.AttenuationMax = Libs.VNX_Atten.GetMaxAttenuation(_devID);
                this.AttenuationStep = Libs.VNX_Atten.GetAttenuationStep(_devID)*0.25;
                this.DwellTime = Libs.VNX_Atten.GetDwellTime(_devID);
                this.IdleTime = Libs.VNX_Atten.GetIdleTime(_devID);
                this.RampEnd = Libs.VNX_Atten.GetRampEnd(_devID)*0.25;
                this.RampStart = Libs.VNX_Atten.GetRampStart(_devID)*0.25;
                this.IsRFOn = (Libs.VNX_Atten.GetRFOn(_devID) == 1) ? true : false;               
            }
        }

        public void ZeroSettings()
        {
            if (_devID != 0)
            {
                this.IsEnabled = false;
                //this.Label = "No device";
                //this._devID = 0;
                //this.SerialNumber = 0;
                //this.Model = "No device";
                this.Attenuation = 0;
                this.AttenuationStep = 2;
                this.RampStart = 0;
                this.RampEnd = 0;
                this.DwellTime = 1000;
                this.IdleTime = 0;
                this.IsRampModeRepeat = false;
                this.IsRFOn = true;
            }
        }

        public void StartConnection()
        {
            if (_devID != 0)
            {
                Libs.VNX_Atten.InitDevice(_devID);
                IsEnabled = true;
            }
        }

        public void CloseConnection()
        {
            if (_devID != 0)
            {
                Libs.VNX_Atten.CloseDevice(_devID);
                IsEnabled = false;
            }
        }

        public void SaveCurrentSettingsAsDefault()
        {
            if (_devID != 0)
                Libs.VNX_Atten.SaveSettings(_devID);
        }

        public string GetModel()
        {
            if (_devID != 0)
            {
                char[] model = new char[32];
                Libs.VNX_Atten.GetModelName(_devID, model);
                return new string(model);
            }
            return "No Device Found";
        }

        private bool CheckMinMaxDB(int val)
        {
            if (val < _attenuationMin )
            {
                //Messenger.Default.Send(new NotificationMessage(Messages.Messages.ATT_LTMIN));
                return false;
            }
            else if (val > _attenuationMax)
            {
                //Messenger.Default.Send(new NotificationMessage(Messages.Messages.ATT_GTMAX));
                return false;
            }
            else 
            {
                return true;
            }
        }

        private void IsSweepUp()
        {
            _isRampDirectionUp = (_rampEnd > _rampStart) ? true : false;
            Libs.VNX_Atten.SetRampDirection(_devID, _isRampDirectionUp);
        }

        /// <summary>
        /// Start the timer to check if attenuation value has changed during a sweep/ramp
        /// </summary>
        /// <param name="interval">in ms</param>
        public void StartRefreshAttTimer(double interval)
        {
            _tmrRefreshAttenuation.Interval = interval;
            _tmrRefreshAttenuation.Enabled = true;
        }
        /// <summary>
        /// Stop the timer to check if attenuation value has changed during a sweep/ramp
        /// </summary>
        public void StopRefreshAttTimer()
        {
            _tmrRefreshAttenuation.Enabled = false;
        }
    }
}
