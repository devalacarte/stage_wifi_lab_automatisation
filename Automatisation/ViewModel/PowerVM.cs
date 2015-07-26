/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Automatisation.Logging.Targets;
using Automatisation.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.ObjectModel;

namespace Automatisation.ViewModel
{
    class PowerVM : ViewModelBase
    {
        #region fields
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        private static string _logTarget = Automatisation.Logging.TargetNames.POWER;
        private Boolean _isLogSet = false;
        private RelayCommand _cmdCreateMeter;
        private RelayCommand _cmdCalibrate;
        private RelayCommand _cmdFetch;
        private PowerMeterModel _pwr;
        private string _lastResult;
        #endregion fields

        #region properties
        public ObservableCollection<string> IncomingMessages { get { return _messages; } private set { if (value != _messages) { _messages = value; } } }
        public RelayCommand CMDCreateMeter { get { return _cmdCreateMeter ?? (_cmdCreateMeter = new RelayCommand(CreateMeter)); } }
        public RelayCommand CMDCalibrate { get { return _cmdCalibrate ?? (_cmdCalibrate = new RelayCommand(Calibrate)); } }
        public RelayCommand CMDFetch { get { return _cmdFetch ?? (_cmdFetch = new RelayCommand(Fetch)); } }
        public string LastResult { get { return _lastResult; } set { if (_lastResult != value) { _lastResult = value; RaisePropertyChanged("LastResult"); } } }
        public PowerMeterModel Pwr { get { return _pwr; } set { if (_pwr != value) { _pwr = value; RaisePropertyChanged("Pwr"); } } }
        #endregion properties

        #region constructor
        /// <summary>
        /// Initializes a new instance of the TabPowerViewModel class.
        /// </summary>
        public PowerVM()
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
                /*case Messages.Messages.POWER_ENABLE:
                    EnablePowerMeter();
                    break;
                case Messages.Messages.POWER_DISABLE:
                    DisablePowerMeter();
                    break;*/
                default:
                    break;
            }
        }
        #endregion events
        /// <summary>
        /// Enable the powermeter and setup logging for this meter.
        /// </summary>
        public void EnablePowerMeter()
        {
            //if (_isLogSet == false)
                //SetupLogging();
            _log.Debug(_logTarget + "EnabledPowerMeter");
            CreateMeter();
        }

        /// <summary>
        /// Disable the powermeter and current logs for this meter. Delete any current results
        /// </summary>
        public void DisablePowerMeter()
        {
            _log.Debug(_logTarget + "DisabledPowerMeter");
            CloseMeter();
            _messages.Clear();
            IncomingMessages.Clear();
            LastResult = string.Empty;
        }

        /// <summary>
        /// setup target listener, listen to logs for the powermeter, add messages to a list
        /// </summary>
        private void SetupLogging()
        {
            //setup target listener
            //var target = Logging.Config.GetTarget(Logging.TargetNames.POWER) as TargetPowerLogs;
            //target.Messages.Subscribe(msg => _messages.Add(msg));
            //IncomingMessages = _messages;
            _isLogSet = true;
        }

        /// <summary>
        /// Create a new powermeter.
        /// Check if the powermeter has a valid IP. If no IP is set, launch the settings menu, else start the connection.
        /// Once the connection is available check if the sensors are connected
        /// </summary>
        private void CreateMeter()
        {
            #region withoutThread
            #endregion withoutThread

            //bool b =await awaitablecommando(....);
            //await Task.Run(mijnmethodedieikopeenanderethreadwiluitvoeren);

            System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Debug(_logTarget + "Creating new PowerMeter"); });
                    Pwr = new PowerMeterModel();
                    Pwr.Ip = SetIP();
                    Pwr.TimeOut = SetTimeout();

                    //Launch settings window if no IP is available
                    if (Pwr.VisaAddress == null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Warn(_logTarget + "IP for PowerMeter not set!"); });
                        Messenger.Default.Send(new NotificationMessage(Messages.Messages.SETTINGSIP_LAUNCH));
                    }
                    //Start connection if IP is available
                    else
                    {
                        try { Pwr.StartConnection(); DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Debug(_logTarget + "Connection successful"); }); }
                        catch (Exception ex) { DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Error(_logTarget + "Connection failed: {0}" + ex.Message, ex); }); return; }
                    }

                    //are sensors connected
                    try { Pwr.GetChannelStatusDescription(); }
                    catch (Exception ex) { DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Error(_logTarget + "GetChannelStatus failed: " + ex.Message, ex); }); }
                }
            );
        }

        /// <summary>
        /// Reset the meter, and close the connection with the device
        /// </summary>
        private void CloseMeter()
        {
            if (Pwr != null)
            {
                Pwr.CloseConnestion();
                Pwr = null;
            }
        }

        /// <summary>
        /// Get the IP from UserSettings
        /// </summary>
        /// <returns></returns>
        private string SetIP()
        {
            string ip = Properties.Settings.Default.IP_AGILENT;
            return (ip != null && ip != string.Empty) ? ip : null;
        }

        /// <summary>
        /// Set the timeout of the powermeter. If no settings are found, standard value of 2 minutes.
        /// </summary>
        /// <returns>timeout in miliseconds</returns>
        private int SetTimeout()
        {
            int tO = Properties.Settings.Default.AGILENT_TIMEOUT;
            return (tO != 0) ? tO : 120000;
        }

        /// <summary>
        /// Zero and Calibrate the powermeter
        /// </summary>
        private void Calibrate()
        {
            string result = String.Empty;
            System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Debug(_logTarget + "Calibration started"); });
                    try
                    {
                        result = Pwr.Calibrate();
                    }
                    catch (Exception e)
                    {
                        result = e.ToString();
                        _log.Error(_logTarget + "Calibration error: " + e.Message, e);
                    }

                    if (int.Parse(result) == 0) { DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Debug(_logTarget + "Calibration success"); }); }
                    else if (int.Parse(result) == 1) { DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Debug(_logTarget + "Calibration failed"); }); }
                    else { DispatcherHelper.CheckBeginInvokeOnUI(() => { _log.Error(_logTarget + "Calibration Error: {0}", result); }); }
                }
            );
        }

        /// <summary>
        /// Fetch the current value on the powermeter
        /// </summary>
        public void Fetch()
        {
            try { LastResult = Pwr.Fetch(); }
            catch (Exception e) { _log.Error(_logTarget + "Error occured while fetching data: " + e.Message, e); return; }
            _log.Debug(_logTarget + "Result for fetching data: {0}", LastResult);
        }

        public string FetchStringResult()
        {
            return Pwr.Fetch();
        }
    }
}
