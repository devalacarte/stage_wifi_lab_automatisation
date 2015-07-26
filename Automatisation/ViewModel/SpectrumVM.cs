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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NLog;
using System;
using System.Collections.ObjectModel;

namespace Automatisation.ViewModel
{
    class SpectrumVM : ViewModelBase
    {
        #region fields
        private RelayCommand _rstConnection;
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private static string _logTarget = Automatisation.Logging.TargetNames.SPECTRUM;
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        private Boolean _isLogSet = false;
        private SpectrumMeterModel _spectrum;
        private string _fq_Center;
        private string _span;
        private string _meas_CHPower_Band;
        private string _avg;
        private string _lastResult;
        #endregion fields

        #region properties
        public RelayCommand ConnectionReset { get { return _rstConnection ?? (_rstConnection = new RelayCommand(Reset)); } }
        public ObservableCollection<string> IncomingMessages { get { return _messages; } private set { if (value != _messages) { _messages = value; } } }
        public SpectrumMeterModel Spectrum { get { return _spectrum; } set { if (_spectrum != value) { _spectrum = value; RaisePropertyChanged("Spectrum"); } } }
        public string FQ_Center { get { return _fq_Center; } set { if (value != _fq_Center) { _fq_Center = value; RaisePropertyChanged("FQ_Center"); } } }
        public string Span { get { return _span; } set { if (value != _span) { _span = value; RaisePropertyChanged("Span"); } } }
        public string Avg { get { return _avg; } set { if (value != _avg) { _avg = value; RaisePropertyChanged("Avg"); } } }
        public string Meas_CHPower_Band { get { return _meas_CHPower_Band; } set { if (value != _meas_CHPower_Band) { _meas_CHPower_Band = value; RaisePropertyChanged("Meas_CHPower_Band"); } } }
        public string LastResult { get { return _lastResult; } set { if (_lastResult != value) { _lastResult = value; RaisePropertyChanged("LastResult"); } } }
        #endregion properties

         #region constructor
        /// <summary>
        /// Initializes a new instance of the TabSpectrumViewModel class.
        /// </summary>
        public SpectrumVM()
        {
            MessengerInstance.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));
            if ((LastResult == null) || (LastResult == string.Empty)) { LastResult = "Not yet fetched"; }
        }
        #endregion constructor

        #region events
        /// <summary>
        /// A handler to do an action when a Message has been received
        /// </summary>
        /// <param name="m">A notification message send through the Messenger service from MVVM Light</param>
        private void NotificationMessageHandler(NotificationMessage m)
        {
            // Checks the actual content of the message.
            switch (m.Notification)
            {
                /*case Messages.Messages.SPECTRUM_ENABLE:
                    EnableSpectrumMeter();
                    break;
                case Messages.Messages.SPECTRUM_DISABLE:
                    DisableSpectrumMeter();
                    break;*/
                default:
                    break;
            }
        }
        #endregion events



        /// <summary>
        /// 
        /// </summary>
        public void EnableSpectrumMeter()
        {
            //if (_isLogSet == false)
                //SetupLogging();
            _log.Debug(_logTarget + "EnabledSpectrumMeter");
            CreateMeter();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisableSpectrumMeter()
        {
            CloseMeter();
            _log.Debug(_logTarget + "DisabledSpectrumMeter");
            IncomingMessages.Clear();
            LastResult = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupLogging()
        {
            //setup target listener
            //var target = Logging.Config.GetTarget(Logging.TargetNames.SPECTRUM) as Automatisation.Logging.Targets.TargetSpectrumLogs;
            //target.Messages.Subscribe(msg => _messages.Add(msg));
            //IncomingMessages = _messages;
            //_isLogSet = true;
            //_log.Debug(_logTarget + "Logging Set");
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateMeter()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Info(_logTarget + "Creating new SpectrumAnalyzer"); });
                    Spectrum = new SpectrumMeterModel();
                    Spectrum.Ip = SetIP();
                    Spectrum.TimeOut = 60000;

                    if (Spectrum.VisaAddress == null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Info(_logTarget + "IP for SpectrumAnalyze not set!"); });
                        Messenger.Default.Send(new NotificationMessage(Messages.Messages.SETTINGSIP_LAUNCH));
                    }
                    else
                    {
                        try
                        {
                            Spectrum.StartConnection();
                            DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Info(_logTarget + "Connection successful"); });
                            GetSettings();
                        }
                        catch (Exception e)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Error(_logTarget + "Connection failed: {0}" + e.Message, e); });
                        }
                    }
                }
            );
        }

        /// <summary>
        /// 
        /// </summary>
        private void CloseMeter()
        {
            if (Spectrum != null)
            {
                Spectrum.CloseConnestion();
                Spectrum = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Reset()
        {
            _log.Info(_logTarget + "Reset Connection Clicked");
            /*   CloseMeter();
               CreateMeter();
               if (Spectrum != null)
               {
                
               }
             * */
            test();
        }
        private void test()
        {
            Automatisation.Commands.Spectrum.FetchSpectrum(_spectrum);
            Spectrum.ReadIEEEBlock(Automatisation.Commands.Spectrum.FETCH_SPECTRUM);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string SetIP()
        {
            string ip = Properties.Settings.Default.IP_TEKTRONIX;
            return (ip != null && ip != string.Empty) ? ip : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int SetTimeout()
        {
            int tO = Properties.Settings.Default.AGILENT_TIMEOUT;
            return (tO != 0) ? tO : 120000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void GetSettings()
        {
            FQ_Center = Commands.Spectrum.GetFreqCenter(_spectrum);
            Span = Commands.Spectrum.GetFreqSpan(_spectrum);
            if (Commands.Spectrum.GetMeas(_spectrum) == "CHP")
                Meas_CHPower_Band = Commands.Spectrum.GetChannelBand(_spectrum);
            else
                Meas_CHPower_Band = "Meas not CHP";
            //Avg = Commands.Spectrum.
        }


        /// <summary>
        /// 
        /// </summary>
        public void StartMeasure()
        {
            /*
             * Checken of da spectrum in continuous mode of single mode staat (repeat)
             * Continuous -> :INIT:CONT? -> 1 -> :abort -> :init:imm -> :init:cont 0 -> :fetch (een read uitvoeren hier als eerste stap zet cont naar 0 en gebruikt de AVERAGE stuff
             * Single -> :INIT:CONT? -> 0 -> :read -> :fetch (read start nieuwe measure en geeft result weer, fetch haald waarde op zonder ,ieuwe measure
             */
            string initCont = Automatisation.Commands.Spectrum.GetInitCont(_spectrum);
            bool isSingleMode;
            if (initCont.Contains("0"))
                isSingleMode = true;
            else if (initCont.Contains("1"))
                isSingleMode = false;
            else
                return;

            if (isSingleMode == true)
            {
                Automatisation.Commands.Spectrum.ReadSpectrum(_spectrum);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetResultSingle()
        {
            string res = Automatisation.Commands.Spectrum.FetchSpectrumCHP(_spectrum);
            if ((res == null) || (res == String.Empty) || (res == ""))
                res = "No result";
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] GetResultArray()
        {
            string[] res = null;
            return res;
        }
    }
}
