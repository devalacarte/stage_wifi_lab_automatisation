using Automatisation.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automatisation.ViewModel
{
    class MainUCVM : ViewModelBase
    {
        private ApplicationVM _appvm;
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        public ObservableCollection<string> IncomingMessages { get { return _messages; } private set { if (value != _messages) { _messages = value; RaisePropertyChanged("IncomingMessages"); } } }
        private RelayCommand _cmdStartMeasure;
        public RelayCommand CMDStartMeasure { get { return _cmdStartMeasure ?? (_cmdStartMeasure = new RelayCommand(StartMeasure, CanStartExecute)); } }
        private bool _bFirstStep = false;
        private bool _firstMeasure = true;
        private NLog.Targets.FileTarget _logFileTarget; 
        #region fields and properties
        private bool _isMeasurementRunning = false;
        public bool IsMeasurementRunning
        {
            get { return _isMeasurementRunning; }
            set { if (_isMeasurementRunning != value) { _isMeasurementRunning = value; RaisePropertyChanged("IsMeasurementRunning"); } }
        }
        
        private string _name = DateTime.Now.ToShortDateString().Replace('/','-');
        public string Name
        {
            get { return _name; }
            set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } }
        }

        private int _ammountOfCycles = 0;
        public int AmmountOfCycles
        {
            get { return _ammountOfCycles; }
            set { if (_ammountOfCycles != value) { _ammountOfCycles = value; RaisePropertyChanged("AmmountOfCycles"); } }
        }

        private int _currentCycle = 0;
        public int CurrentCycle
        {
            get { return _currentCycle; }
            set { if (_currentCycle != value) { _currentCycle = value; RaisePropertyChanged("CurrentCycle"); } }
        }

        private bool _isEveryAttSameSetting = false;
        public bool IsEveryAttSameSetting
        {
            get { return _isEveryAttSameSetting; }
            set { if (_isEveryAttSameSetting != value) { _isEveryAttSameSetting = value; RaisePropertyChanged("IsEveryAttSameSetting"); if (!value) { SetAttDefaultSettings(); } } }
        }

        private int _waitForAtt;
        public int WaitForAtt
        {
            get { return _waitForAtt; }
            set { if (_waitForAtt != value) { _waitForAtt = value; RaisePropertyChanged("WaitForAtt"); } }
        }

        /*private Nullable<double> _setAtt;
        public Nullable<double> SetAtt
        {
            get { return _setAtt; }
            set { if (!IsEveryAttSameSetting) return; if (_setAtt != value) { _setAtt = value; RaisePropertyChanged("SetAtt"); } }
        }*/

        private Nullable<double> _setStart;
        public Nullable<double> SetStart
        {
            get { return _setStart; }
            set { if (!IsEveryAttSameSetting) return; if (_setStart != value) { _setStart = value; RaisePropertyChanged("SetStart"); } }
        }

        private Nullable<double> _setStop;
        public Nullable<double> SetStop
        {
            get { return _setStop; }
            set { if (!IsEveryAttSameSetting) return; if (_setStop != value) { _setStop = value; RaisePropertyChanged("SetStop"); } }
        }

        private Nullable<double> _setStep;
        public Nullable<double> SetStep
        {
            get { return _setStep; }
            set { if (!IsEveryAttSameSetting) return; if (_setStep != value) { _setStep = value; RaisePropertyChanged("SetStep"); } }
        }

        private int _waitForIperf;
        public int WaitForIperf
        {
            get { return _waitForIperf; }
            set { if (_waitForIperf != value) { _waitForIperf = value; RaisePropertyChanged("WaitForIperf"); } }
        }

        private string _iperfServerCommando;
        public string IperfServerCommando
        {
            get { return _iperfServerCommando; }
            set { if (_iperfServerCommando != value) { _iperfServerCommando = value; RaisePropertyChanged("IperfServerCommando");
            /*_iperfTArg = (GetValueForArgumentIperf(value,"-t")==0)?120:GetValueForArgumentIperf(value,"-t");*/ } }
        }

        private string _iperfClientCommando;
        public string IperfClientCommando
        {
            get { return _iperfClientCommando; }
            set { if (_iperfClientCommando != value) { _iperfClientCommando = value; RaisePropertyChanged("IperfClientCommando"); 
                WaitForIperf = (GetValueForArgumentIperf(value,"-t")==0)?120:GetValueForArgumentIperf(value,"-t"); } 
            }
        }

        private bool _isServerWindows;
        public bool IsServerWindows
        {
            get { return _isServerWindows; }
            set { if (_isServerWindows != value) { _isServerWindows = value; RaisePropertyChanged("IsServerWindows"); } }
        }

        private bool _isClientWindows;
        public bool IsClientWindows
        {
            get { return _isClientWindows; }
            set { if (_isClientWindows != value) { _isClientWindows = value; RaisePropertyChanged("IsClientWindows"); } }
        }
        #endregion fields and properties

        private RelayCommand _viewLoaded; private bool _bViewLoaded = false;
        public RelayCommand ViewLoaded { get { return _viewLoaded ?? (_viewLoaded = new RelayCommand(ControlLoaded)); } }
        private void ControlLoaded()
        { _appvm = App.Current.MainWindow.DataContext as ApplicationVM; _bViewLoaded = true; }
        public MainUCVM()
        {
            SetupLogging();
        }
        private void SetupLogging()
        {
            //var target = Logging.Config.GetTarget(Logging.TargetNames.ALL) as Automatisation.Logging.Targets.TargetAllLogs;
            //target.Messages.Subscribe(msg => _messages.Add(msg));
           // IncomingMessages = _messages;
        }



        private AutoResetEvent autoEventRestartIperf = new AutoResetEvent(false);
        //auto.set(); //auto.waitone();
        //private AutoResetEvent autoWaitForFinish = new AutoResetEvent(false);
        private void StartMeasure()
        {
            if (_firstMeasure)
                LoggingSubscribe();
            IncomingMessages.Clear();
            IsMeasurementRunning = true;
            SetNLOGLoggingToFile();
            ApplicationVM.LOG.Debug("------------------------------------Start measurement------------------------------------");
            ApplicationVM.LOG.Debug("checking if settings are filled in.");
            //check if settings are filled in
            if (!CheckIfAllSettingsAreFilledIn())
            {
                ApplicationVM.LOG.Warn("Check settings on Measure Tab");
                IsMeasurementRunning = false;
                return;
            }
            else
                ApplicationVM.LOG.Debug("Settings OK");

            //setting attenuators start position
            if (this.IsEveryAttSameSetting)
                SetAttSameSettings();
            
            //log wich devices to use + their settings
            LogSettingsToUse();
            
            //ApplicationVM.LOG.Debug("Start Iperf-server");

            //Thread tIperf = new Thread(RestartIperf);
            //tIperf.Start();
            //start the measurement, set attenuators, run iperf, get spectrum and power data
            Thread tMeasure = new Thread (Measure);
            RestartIperf();
            //Measure();
            tMeasure.Start();

            //autoWaitForFinish.WaitOne();
            //ApplicationVM.LOG.Info("------------------------------------Measurement {0} finished on {1}------------------------------------", Name, DateTime.Now.ToString());
            //IsMeasurementRunning = false;
        }

        private bool CanStartExecute()
       {
           if (_bViewLoaded == false)
               return false;

            if (CheckIfAllSettingsAreFilledIn() && IsMeasurementRunning==false)
                return true;
            else
                return false;
        }
        private bool CanStopExecute()
        {
            return IsMeasurementRunning;
        }
        private void SetNLOGLoggingToFile()
        {
            //check if file already exists -> new filename
            
            NLog.Config.LoggingConfiguration config = NLog.LogManager.Configuration;
            //config.
            
            if (_firstMeasure)
            {
                _logFileTarget = new NLog.Targets.FileTarget();
                _logFileTarget.Name = Logging.TargetNames.FILE;
                _logFileTarget.CreateDirs = true;
                _logFileTarget.KeepFileOpen = true;
                _logFileTarget.Layout = "${message}";
                _logFileTarget.FileName = string.Format("{0}\\{1}.txt", Properties.Settings.Default.NLOG_LOGFILE_LOCATION, Name);
                config.AddTarget(Logging.TargetNames.FILE, _logFileTarget);
                config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Info, _logFileTarget));
            }
            else
            {
                /*if (Logging.Config.GetTarget(Logging.TargetNames.FILE) != null)
                    config.RemoveTarget(Logging.TargetNames.FILE);*/
                try
                {
                    config.RemoveTarget(Logging.TargetNames.FILE);
                }
                catch (Exception)
                {
                    //throw;
                }
                _logFileTarget.FileName = string.Format("{0}\\{1}.txt", Properties.Settings.Default.NLOG_LOGFILE_LOCATION, Name);
                config.AddTarget(Logging.TargetNames.FILE, _logFileTarget);
            }
            NLog.LogManager.Configuration = config;   
        }
        private void LoggingSubscribe()
        {
            var tar = Logging.Config.GetTarget(Logging.TargetNames.ALL) as Automatisation.Logging.Targets.TargetAllLogs;
            tar.Messages.Subscribe(msg => _messages.Add(msg));
            IncomingMessages = _messages;
        }

        private void SetAttDefaultSettings()
        {
            _appvm.TabAtt.ResetAllToDefault();
        }
        private void SetAttSameSettings()
        {
            _appvm.TabAtt.SetEveryAttSameSetting(/*(double)SetAtt*/(double)SetStart, (double)SetStart, (double)SetStop, (double)SetStep);
        }
        private int GetValueForArgumentIperf(string command, string arg)
        {
            string teststring;
            if (command.IndexOf(arg)==-1)
                return 0;
            
            teststring = command.Substring(command.IndexOf("-t"));

            if (teststring.Substring(2, 1) == " ") // of char.IsWhiteSpace
                teststring = teststring.Substring(3);
            else
                teststring = teststring.Substring(2);

            return int.Parse(teststring.Split(' ')[0]);
        }
        private bool CheckIfAllSettingsAreFilledIn()
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            if (AmmountOfCycles <= 0)
                return false;

            if (_appvm.IsSpectrumEnabled)
            {
                if (string.IsNullOrEmpty(_appvm.TabSpectrum.FQ_Center) || string.IsNullOrEmpty(_appvm.TabSpectrum.Span) || string.IsNullOrEmpty(_appvm.TabSpectrum.Meas_CHPower_Band))
                    return false;
            }

            if (_appvm.IsAttEnabled)
            {
                if( WaitForAtt <=0)
                    return false;
                if (this.IsEveryAttSameSetting)
                {
                    if (/*SetAtt == null || */SetStart == null || SetStop == null || SetStep == null)
                        return false;
                }
            }
            if (_appvm.IsIperfEnabled)
            {
                /*if (WaitForIperf <= 0)
                    return false;*/
                if (string.IsNullOrEmpty(IperfClientCommando) || string.IsNullOrEmpty(IperfServerCommando))
                    return false;
            }
            return true;
        }
        private void LogSettingsToUse()
        {
            ApplicationVM.LOG.Info("Test and File name: {0}",this.Name);
            ApplicationVM.LOG.Info("Started on: {0}; Ammount of cylces:{1}",DateTime.Now.ToString(),AmmountOfCycles);
            if (_appvm.IsPowerEnabled)
                ApplicationVM.LOG.Info("Powermeter: enabled");
            if (_appvm.IsSpectrumEnabled)
                ApplicationVM.LOG.Info("Spectrumanalyzer: enabled; Center:{0}; Span:{1}; Band:{2}", _appvm.TabSpectrum.FQ_Center, _appvm.TabSpectrum.Span, _appvm.TabSpectrum.Meas_CHPower_Band);
            if (_appvm.IsAttEnabled)
            {
                ApplicationVM.LOG.Info(LogCheckWichAttenuatorsAreEnabled());
                //ApplicationVM.LOG.Info(LogAttSetting());
            }
            if (_appvm.IsIperfEnabled)
                ApplicationVM.LOG.Info("Iperf: enabled; Server:{0}; Client:{1}; Wait:{2}",this.IperfServerCommando,this.IperfClientCommando, this.WaitForIperf);
        }
        private string LogCheckWichAttenuatorsAreEnabled()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Attenuators which are enabled: ");
            foreach (AttenuatorUCVM item in _appvm.TabAtt.LstAtt)
            {
                if (item.Att.IsEnabled)
                    sb.Append(item.Att.Label).Append(", ");
            }
            sb.ToString().Substring(sb.ToString().Length - 2).Replace(", ", string.Empty); //verwijder laatste (", ")
            return sb.ToString();
        }
        private string LogAttSetting()
        {
            if (this.IsEveryAttSameSetting)
                return string.Format("Attenuator have the same settings: Start:{0}; Stop{1}; Step{2};", SetStart, SetStop, SetStep);
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Attenuators have different settings");
                foreach (AttenuatorUCVM item in _appvm.TabAtt.LstAtt)
                {
                    if (item.Att.IsEnabled)
                        sb.AppendLine(string.Format("Attenuator {0}: Start:{0}; Stop{1}; Step{2};", SetStart, SetStop, SetStep));
                }
                return sb.ToString();
            }
                
        }
        private string LogAttDbSettings()
        {
            StringBuilder sb = new StringBuilder();
            foreach (AttenuatorUCVM item in _appvm.TabAtt.LstAtt)
            {
                if (item.Att.IsEnabled)
                    sb.AppendLine(string.Format("Attenuator {0} ingesteld op {1} dB.", item.Att.Label, item.Att.Attenuation));
            }
            return sb.ToString();
        }

        private void RestartIperf()
        {
            string[] sKillIperf = (IsServerWindows) ? new string[] { "taskkill /im iperf.exe /F" } : new string[] { "killall -15 iperf" };
            NotificationMessage<string[]> nm = new NotificationMessage<string[]>(sKillIperf, Messages.Messages.IPERF_SERVER_CMD);
            Messenger.Default.Send(nm);
            Thread.Sleep(2000);
            Messenger.Default.Send(new NotificationMessage<string[]>(new string[] { IperfServerCommando }, Messages.Messages.IPERF_SERVER_CMD));
            Thread.Sleep(2000);
            autoEventRestartIperf.Set();
        }

        private void Measure()
        {
            _firstMeasure = false;
            autoEventRestartIperf.WaitOne();
            ApplicationVM.LOG.Info("Begin measurement");

            Thread tExecute;
            if (IsEveryAttSameSetting)
               tExecute = new Thread(ExecuteSameAttSettings);
            else
               tExecute = new Thread(ExecuteDifferentAttSettings);

            tExecute.Start();
        }
        
        private void ExecuteDifferentAttSettings()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("Executing program untill ammount of steps has been reached"); });
            for (int i = 0; i < AmmountOfCycles; i++)
            {
                int ii = i; //reden: gaf altijd nieuwe waarde mee in de log
                _bFirstStep = (i == 0) ? true : false;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("--------------------------Starting step number:{0}-------------------------", ii); });
                ExecuteStep();
                DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("-------------------------Step number {0} completed-------------------------", ii); });
            }
            //autoWaitForFinish.Set();
            DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("------------------------------------Measurement {0} finished on {1}------------------------------------", Name, DateTime.Now.ToString()); });
            IsMeasurementRunning = false;
        }


        private void ExecuteSameAttSettings()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("Looping program untill {0} cycles have been completed", AmmountOfCycles); });
            for (int i = 0; i < AmmountOfCycles; i++)
            {
                _bFirstStep = true;
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    ApplicationVM.LOG.Info("--------------------------------------------------------------------------");
                    ApplicationVM.LOG.Info("-----------------------------Starting cycle {0}-----------------------------", i);
                    ApplicationVM.LOG.Info("--------------------------------------------------------------------------");
                });
                if(_appvm.IsAttEnabled)
                    SetAttSameSettings();
                for (double d = (double)SetStart; d < (SetStop+SetStep); d += (double)SetStep) //zonder +setstep bij d< setstop, stopt de loop bij de voorlaatste step. bv: 10-14 stappe van 2 = 10 en 12 ipv 10, 12 en 14
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("-------------------Starting step with attenuation:{0} dB-------------------", d); });
                        ExecuteStep();
                    _bFirstStep = false;
                    double dd = d;//reden: gaf altijd nieuwe waarde mee in de log
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { 
                        ApplicationVM.LOG.Info("-------------------Step with attenuation:{0} dB completed------------------", dd); 
                    });
                }
                int ii = i; //reden: gaf altijd nieuwe waarde mee in de log
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    ApplicationVM.LOG.Info("--------------------------------------------------------------------------");
                    ApplicationVM.LOG.Info("------------------------------Ending cycle {0}------------------------------", ii);
                    ApplicationVM.LOG.Info("--------------------------------------------------------------------------");
                });
                //autoWaitForFinish.Set();
            }
            DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("------------------------------------Measurement {0} finished on {1}------------------------------------", Name, DateTime.Now.ToString()); });
            IsMeasurementRunning = false;
        }
        
        private void ExecuteStep()
        {
            if (_appvm.IsAttEnabled)
            {
                if (!_bFirstStep)
                    _appvm.TabAtt.NextStepAll();

                DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("Setting attenuators, wait for {0} seconds", WaitForAtt); });
                Thread.Sleep(WaitForAtt * 1000);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    ApplicationVM.LOG.Info("Attenuators set");
                    ApplicationVM.LOG.Info(LogAttDbSettings());
                });
            }

            string PowerMeterResult = "";

            if (_appvm.IsIperfEnabled)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => { ApplicationVM.LOG.Info("Start Iperf: {0}", IperfClientCommando); });

                string[] sarr = new string[] { IperfClientCommando };
                NotificationMessage<string[]> nm = new NotificationMessage<string[]>(sarr, Messages.Messages.IPERF_CLIENT_CMD);
                Messenger.Default.Send(nm);
                
                //powermeter starten in het midden van de iperfmeting
                if (_appvm.IsPowerEnabled)
                {
                    Thread.Sleep((WaitForIperf * 1000) / 2);
                    try
                    {
                        PowerMeterResult = _appvm.TabPwr.FetchStringResult();
                    }
                    catch (Exception e)
                    {
                        PowerMeterResult = "Error occured while fetching powermeter: " + e.Message;
                    }
                    Thread.Sleep(((WaitForIperf * 1000) / 2) +10000);
                }
                else
                {
                    Thread.Sleep((WaitForIperf * 1000) + 10000); //ophalen uit commando
                }
                

                
                DispatcherHelper.CheckBeginInvokeOnUI(() => { 
                    ApplicationVM.LOG.Info("Iperf ended", IperfClientCommando);
                    ApplicationVM.LOG.Info("Powermeter result: {0}", PowerMeterResult); 
                });
            }

            //5 seconden wachten
            //power en spectrum uitlezen tegelijkertijd
            //meet object aanmaken met alle resultaten om dit object op te slaan in een database
        }
    }
}
