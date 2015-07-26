/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Automatisation.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    class AttenuatorsVM : ObservableObject
    {
        #region fields
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private ObservableCollection<AttenuatorUCVM> _lstAtt;
        private int _connectedDevices;
        private uint[] _myDevices;
        /// <summary>
        /// 1 = testmode, 0 = look for real attenuators
        /// </summary>
        private int _isTestMode = 0;
        private RelayCommand _cmdRefresh;
        private RelayCommand _cmdStartAll;
        private RelayCommand _cmdStopAll;
        private RelayCommand _cmdManualNextStep;
        #endregion fields

        #region properties
        /// <summary>
        /// Observable collection to store found attenuators
        /// </summary>
        public ObservableCollection<AttenuatorUCVM> LstAtt { get { return _lstAtt; } set { if (value != _lstAtt) { _lstAtt = value; OnPropertyChanged("LstAtt"); } } }
        /// <summary>
        /// Ammount of connected attenuators
        /// </summary>
        public int ConnectedDevices
        {
            get { return _connectedDevices; }
            set
            {
                if (value != _connectedDevices)
                {
                    _connectedDevices = value;
                    MyDevices = new uint[ConnectedDevices];
                    Libs.VNX_Atten.GetDevInfo(MyDevices);
                    OnPropertyChanged("ConnectedDevices");
                }
            }
        }
        /// <summary>
        /// Array of uint to store device ids.
        /// </summary>
        public uint[] MyDevices { get { return _myDevices; } set { if (value != _myDevices) { _myDevices = value; OnPropertyChanged("ConnectedDevices"); } } }

        public RelayCommand CMDRefresh { get { return _cmdRefresh ?? (_cmdRefresh = new RelayCommand(RefreshAll)); } }
        public RelayCommand CMDStartAll { get { return _cmdStartAll ?? (_cmdStartAll = new RelayCommand(StartAll)); } }
        public RelayCommand CMDStopAll { get { return _cmdStopAll ?? (_cmdStopAll = new RelayCommand(StopAll)); } }
        public RelayCommand CMDManualNextStep { get { return _cmdManualNextStep ?? (_cmdManualNextStep = new RelayCommand(NextStepAll)); } }
        #endregion properties

        #region constructor
        /// <summary>
        /// Initializes a new instance of the TabAttenuatorViewModel class.
        /// </summary>
        public AttenuatorsVM()
        {
            //Messenger.Default.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));
        }
        #endregion constructor

       
        /// <summary>
        /// Enable attenuator functions: Look for attenuators and connect them
        /// </summary>
        public void EnableAttenuators()
        {
            if (_isTestMode == 1)
                Messenger.Default.Send(new NotificationMessage(Messages.Messages.ATTENUATOR_TEST_MODE));
            Libs.VNX_Atten.SetTestMode(_isTestMode);
            //Task task = new Task(() =>
            //{
                LookForDevices();
                ConnectAllDevices();
           // });
            //task.Start();
            //if (_isTestMode == 1)
                //GenerateExtraDevicedTestLayout();
        }
        private void GenerateExtraDevicedTestLayout()
        {
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
            LstAtt.Add(new AttenuatorUCVM());
        }
        /// <summary>
        /// Disable attenuator functions: Disconnect every attenuator
        /// </summary>
        public void DisableAttenuators()
        {
            DisconnectAllDevices();
            LstAtt.Clear();
            LstAtt = null;
        }

        /// <summary>
        /// Look for usb connected attenuators, create model object and add to a collection
        /// </summary>
        private void LookForDevices()
        {
            //get ammount of connected devices. If > 0 create an attuenator object for each device and add it to a collection
            ConnectedDevices = Libs.VNX_Atten.GetNumDevices();
            if (ConnectedDevices != 0)
            {
                LstAtt = new ObservableCollection<AttenuatorUCVM>();
                foreach (uint dev in _myDevices)
                {
                    AttenuatorUCVM att = new AttenuatorUCVM(dev);
                    LstAtt.Add(att);
                }
            }
        }

        /// <summary>
        /// Loop through a collection of Attenuators and init every device
        /// </summary>
        private void ConnectAllDevices()
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                att.StartConnection(); att.GetSettings();
            }
        }

        /// <summary>
        /// Loop through a collection of Attenuators and close the connection with every device
        /// </summary>
        private void DisconnectAllDevices()
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                att.CloseConnection();
            }
        }

        /// <summary>
        /// Loop through a collection of Attenuators and close the connection with given device
        /// </summary>
        /// <param name="devid">Device id of the device to disconnect</param>
        private void DisconnectDeviceRemoveFromList(uint devid)
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.DevID == devid)
                {
                    att.CloseConnection();
                    LstAtt.Remove(att);
                }
            }
        }

        /// <summary>
        /// Get Attenuator object from list by device id
        /// </summary>
        /// <param name="devid"></param>
        /// <returns></returns>
        private AttenuatorUCVM GetAttByIdFromList(uint devid)
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.DevID == devid)
                {
                    return att;
                }
            }
            return null;
        }

        /// <summary>
        /// Loop through a collection of Attenuators and set settings to zero for every device
        /// </summary>
        /// <param name="devid"></param>
        private void ResetToZeroSettings(/*uint dev*/)
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.Att.IsEnabled)
                    att.ZeroSettings();
            }
        }

        public void ResetAllToDefault()
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.Att.IsEnabled)
                    att.DefaultSettings();
            }
        }

        /// <summary>
        /// Looks for devices, fills up list of attenuator objects and connects each object/device for communication
        /// </summary>
        private void RefreshAll()
        {
            LookForDevices();
            ConnectAllDevices();
        }

        /// <summary>
        /// For every device in list: when repeat is set,check if rampdirection is up|down and add|substract attenuationstep to|from attenuation
        /// </summary>
        public void NextStepAll()
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.Att.IsEnabled == true)
                {
                    att.Next();
                }
            }
        }

        /// <summary>
        /// Start sweepingfor every enabled device
        /// </summary>
        public void StartAll()
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.Att.IsEnabled)
                    att.Start();
            }
        }

        /// <summary>
        /// Stop sweeping for every disabled device
        /// </summary>
        public void StopAll()
        {
            foreach (AttenuatorUCVM att in LstAtt)
            {
                if (att.Att.IsEnabled)
                    att.Stop();
            }
        }

        public void SetEveryAttSameSetting(double att, double start, double stop, double step)
        {
            foreach (AttenuatorUCVM attUC in LstAtt)
            {
                if (attUC.Att.IsEnabled)
                {
                    attUC.Att.Attenuation = att;
                    attUC.Att.RampStart = start;
                    attUC.Att.RampEnd = stop;
                    attUC.Att.AttenuationStep = step;
                }
            }
        }

        /// <summary>
        /// Used to store label names for serial numbers
        /// </summary>
        public enum SerialNumbers : int
        {
            //Panel1 aan de kast
            /// <summary>
            /// Serial number for attenuator 1 on panel 1: 5491
            /// </summary>
            ATTENUATOR1 = 5491,
            /// <summary>
            /// Serial number for attenuator 2 on panel 1: 5491
            /// </summary>
            ATTENUATOR2 = 5489,
            /// <summary>
            /// Serial number for attenuator 3 on panel 1: 5491
            /// </summary>
            ATTENUATOR3 = 5492,
            /// <summary>
            /// Serial number for attenuator 4 on panel 1: 5491
            /// </summary>
            ATTENUATOR4 = 5495,

            //Panel2 aan de muur achter de kast
            /// <summary>
            /// Serial number for attenuator 1 on panel 2: 5501
            /// </summary>
            ATTENUATOR5 = 5501,
            /// <summary>
            /// Serial number for attenuator 2 on panel 2: 5499
            /// </summary>
            ATTENUATOR6 = 5499,
            /// <summary>
            /// Serial number for attenuator 3 on panel 2: 5497
            /// </summary>
            ATTENUATOR7 = 5497,
            /// <summary>
            /// Serial number for attenuator 4 on panel 2: 5496
            /// </summary>
            ATTENUATOR8 = 5496,

            //Panel3 aan de muur in het midden
            /// <summary>
            /// Serial number for attenuator 1 on panel 3: 5493
            /// </summary>
            ATTENUATOR9 = 5493,
            /// <summary>
            /// Serial number for attenuator 2 on panel 3: 5500
            /// </summary>
            ATTENUATOR10 = 5500,
            /// <summary>
            /// Serial number for attenuator 3 on panel 3: 5498
            /// </summary>
            ATTENUATOR11 = 5498,
            /// <summary>
            /// Serial number for attenuator 4 on panel 3: 5494
            /// </summary>
            ATTENUATOR12 = 5494,

            /// <summary>
            /// Virtual Attenuator 1 (in testmode)
            /// </summary>
            TEST1 = 55102,
            /// <summary>
            /// Virtual Attenuator 2 (in testmode)
            /// </summary>
            TEST2 = 55602
        }
    }
}