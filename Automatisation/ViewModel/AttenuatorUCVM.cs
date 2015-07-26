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
using System;

namespace Automatisation.ViewModel
{
    class AttenuatorUCVM : ObservableObject
    {

        private double _attenuationBackup;
        private double _attenuationStepBackup;
        private int _dwellTimeBackup;
        private int _idleTimeBackup;
        private double _rampStartBackup;
        private double _rampEndBackup;

        private uint _devID;
        public uint DevID
        {
            get { return _devID; }
            set { if (_devID != value) { _devID = value; OnPropertyChanged("DevID"); } }
        }

        private string _label;
        public string Label { get { return _label; } set { if (value != _label) { _label = value; OnPropertyChanged("Label"); } } }

        private RelayCommand _cmdSave;
        private RelayCommand _cmdStart;
        private RelayCommand _cmdStop;
        private RelayCommand _cmdSet;
        private RelayCommand _cmdDefault;
        private RelayCommand _cmdNext;
        public RelayCommand CMDSave { get { return _cmdSave ?? (_cmdSave = new RelayCommand(Save)); } }
        public RelayCommand CMDStart { get { return _cmdStart ?? (_cmdStart = new RelayCommand(Start)); } }
        public RelayCommand CMDStop { get { return _cmdStop ?? (_cmdStop = new RelayCommand(Stop)); } }
        public RelayCommand CMDSet { get { return _cmdSet ?? (_cmdSet = new RelayCommand(Set)); } }
        public RelayCommand CMDDefault { get { return _cmdDefault ?? (_cmdDefault = new RelayCommand(DefaultSettings)); } }
        public RelayCommand CMDNext { get { return _cmdNext ?? (_cmdNext = new RelayCommand(Next)); } }

        private Attenuator _att;
        public Attenuator Att
        {
            get { return _att; }
            set { if (_att != value) { _att = value; OnPropertyChanged("Att"); } }
        }
        public AttenuatorUCVM() { }
        public AttenuatorUCVM(uint id)
        {
            DevID = id;
            Att = new Attenuator(DevID);
            StartConnection();
            GetSettings();
            foreach (var val in Enum.GetValues(typeof(Automatisation.ViewModel.AttenuatorsVM.SerialNumbers)))
            {
                if (Att.SerialNumber == (int)val)
                {
                    Att.Label = val.ToString();
                    Label = Att.Label;
                }
            }
            BackupSettings();
        }

        /// <summary>
        /// Save current settings for device with parameter devid as default settings
        /// </summary>
        /// <param name="devid"></param>
        public void Save()
        {
            Libs.VNX_Atten.SaveSettings(DevID);
            BackupSettings();
        }

        /// <summary>
        /// Start sweep for a single device
        /// </summary>
        /// <param name="devid">DeviceID from attenuator</param>
        public void Start()
        {
            //Attenuator _att = new Attenuator(DevID);
            Libs.VNX_Atten.SetRampMode(DevID, Att.IsRampModeRepeat);
            Libs.VNX_Atten.SetRampDirection(DevID, Att.IsRampDirectionUp);
            Libs.VNX_Atten.StartRamp(DevID, true);
            Att.StartRefreshAttTimer(Att.DwellTime / 2);
        }

        /// <summary>
        /// Stop sweep for a single device
        /// </summary>
        /// <param name="devid">DeviceID from attenuator</param>
        public void Stop()
        {
            Libs.VNX_Atten.StartRamp(DevID, false);
            Att.StopRefreshAttTimer();
        }

        public void Set()
        {
            Att.RefreshAttenuation();
        }



        public void GetSettings()
        {
            Att.GetSettings();
        }

        public void BackupSettings()
        {
            _attenuationBackup = Att.Attenuation;
            _attenuationStepBackup = Att.AttenuationStep;
            _dwellTimeBackup = Att.DwellTime;
            _idleTimeBackup = Att.IdleTime;
            _rampStartBackup = Att.RampStart;
            _rampEndBackup = Att.RampEnd;
        }

        public void DefaultSettings()
        {
            Att.Attenuation = _attenuationBackup;
            Att.AttenuationStep = _attenuationStepBackup;
            Att.DwellTime = _dwellTimeBackup;
            Att.IdleTime = _idleTimeBackup;
            Att.RampStart = _rampStartBackup;
            Att.RampEnd = _rampEndBackup;
        }

        public void StartConnection()
        {
            Att.StartConnection();
        }

        public void CloseConnection()
        {
            Att.CloseConnection();
        }

        public void ZeroSettings()
        {
            Att.ZeroSettings();
        }

        public void Next()
        {
            double newAttenuation = Att.Attenuation;
            //check the ramp direction and add or substract the attenuation step
            //when repeat is set,check if rampdirection is up|down and add|substract attenuationstep to|from attenuation
            if (Att.IsRampDirectionUp == true /*&& _att.IsRampModeRepeat == true*/)
            {
                newAttenuation += Att.AttenuationStep;
                if (newAttenuation >= Att.RampEnd + Att.AttenuationStep)
                    newAttenuation = Att.RampStart;
            }
            else if (Att.IsRampDirectionUp == false)
            {
                newAttenuation -= Att.AttenuationStep;
                if (newAttenuation <= Att.RampEnd - Att.AttenuationStep)
                    newAttenuation = Att.RampStart;
            }
            Att.Attenuation = newAttenuation;
        }
    }
}
