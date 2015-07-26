/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using NLog;


namespace Automatisation.Model
{
    public class SettingsSpectrumModel : BaseModel
    {
        #region Fields
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private SpectrumMeterModel _spectrum;
        private bool _blnAllowSend = false; //set to false during the first phase -> getting all the settings. Set on true when actually changing settings
        private bool _blnAllowChannelChange; //When no table is selected (NONE), then no channels can be set.

        //frequency settings (freq/channel)
        private string _fq_Center;
        private string _fq_Start;
        private string _fq_Stop;

        //channel settings (freq/channel)
        private string _ch;
        private string _chTable;
        private string[] _chTableArray;
        private string _chTableSelected;

        //span settings (span)
        private string _span;
        private string _span_Start;
        private string _span_Stop;

        //step
        private string _fq_step;

        //trig settings (trig)
        private string _trigger_Repeat;

        //Amplitude settings (amplitude)
        private string _ampl_RefLevel;
        private string _ampl_AutoOrMixerDescription;
        private bool _ampl_IsAutoAndMixer;
        private string _ampl_RFAtt;
        private string _ampl_MixLvl;
        private string _ampl_VertScale;

        //RBW (rbw/fft)
        private string _rbwOrFFT;
        private bool _rbwMan;
        private bool _fftMan;
        private string _rbw_Hz;
        private string _rbw_FilterShape;
        //fft (rbw/fft)
        private string _fft_Points;
        private string _fft_Windows;
        private string _extended_Res;

        //Trace Trace/AVG
        private string _trc;
        private string _trc_NumberOfTraces;
        private string _trc_DisplayDetection;

        //Measure
        private string _measure;
        private string _meas_CHPower_Band;
        private string _meas_CHPower_FilterShape;
        private string _meas_CHPower_RollOff;
        #endregion Fields
        
        #region properties
        /// <summary>
        /// Allow WriteCommands of any kind to the spectrum analyzer.
        /// </summary>
        public bool AllowSend { 
            get { return _blnAllowSend; } 
            set { if (value != _blnAllowSend) { _blnAllowSend = value; } } }
        /// <summary>
        /// When no table is selected (NONE), then no channels can be set.
        /// </summary>
        public bool AllowChannelChange { 
            get { return _blnAllowChannelChange; } 
            set { if (value != _blnAllowChannelChange) { _blnAllowChannelChange = value; OnPropertyChanged("AllowChannelChange"); } } }

        //frequency settings (freq/channel)
        public string FQ_Center { 
            get { return _fq_Center; } 
            set { if (value != _fq_Center) { 
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFreqCenter(_spectrum, value); } 
                _fq_Center = value; OnPropertyChanged("FQ_Center"); } } }
        public string FQ_Start { 
            get { return _fq_Start; } 
            set { if (value != _fq_Start) { 
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFreqStart(_spectrum, value); } 
                _fq_Start = value; OnPropertyChanged("FQ_Start"); } } }
        public string FQ_Stop { 
            get { return _fq_Stop; } 
            set { if (value != _fq_Stop) { 
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFreqStop(_spectrum, value); } 
                _fq_Stop = value; OnPropertyChanged("FQ_Stop"); } } }

        //channel settings (freq/channel)
        public string Ch { 
            get { return _ch; } 
            set { if (value != _ch) { 
                _ch = value; OnPropertyChanged("Ch"); } } }
        public string ChTable { 
            get { return _chTable; } 
            set { if (value != _chTable) { 
                _chTable = value; ChTableArray = _chTable.Split(','); OnPropertyChanged("ChTable"); } } }
        public string[] ChTableArray { 
            get { return _chTableArray; } 
            set { if (value != _chTableArray) { 
                _chTableArray = value; OnPropertyChanged("ChTableArray"); } } }
        public string ChTableSelected { 
            get { return _chTableSelected; } 
            set { if (value != _chTableSelected) {
                //if value contains NONE then channel can't be set
                string s = value.ToUpper(); bool b = s.Contains("\"NONE\"");
                if (b == true) { AllowChannelChange = false; } else { AllowChannelChange = true; }
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFreqChanCat(_spectrum, value); ChannelTableChanged(); } 
                _chTableSelected = value; OnPropertyChanged("ChTableSelected"); } } }

        //step
        public string FQ_Step { 
            get { return _fq_step; } 
            set { if (value != _fq_step) { 
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFreqCenterStepAutoIncr(_spectrum, value); } 
                _fq_step = value; OnPropertyChanged("FQ_Step"); } } }

        //span settings (span)
        public string Span { 
            get { return _span; } 
            set { if (value != _span) { 
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFreqSpan(_spectrum, value); } 
                _span = value; OnPropertyChanged("Span"); } } }
        public string Span_Start { 
            get { return _span_Start; } 
            set { if (value != _span_Start) { 
                _span_Start = value; OnPropertyChanged("Span_Start"); } } }
        public string Span_Stop { 
            get { return _span_Stop; } 
            set { if (value != _span_Stop) { 
                _span_Stop = value; OnPropertyChanged("Span_Stop"); } } }

        //trig settings (trig)
        public string Trigger_Repeat { 
            get { return _trigger_Repeat; } 
            set { if (value != _trigger_Repeat) { 
                _trigger_Repeat = value; OnPropertyChanged("Trigger_Repeat"); } } }

        //Amplitude settings (amplitude)
        public string Ampl_RefLevel { 
            get { return _ampl_RefLevel; } 
            set { if (value != _ampl_RefLevel) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetAmplRefLvl(_spectrum, value); } 
                _ampl_RefLevel = value; OnPropertyChanged("Ampl_RefLevel"); } } }
        public string Ampl_AutoOrMixerDescription { 
            get { return _ampl_AutoOrMixerDescription; } 
            set { if (value != _ampl_AutoOrMixerDescription) { 
                _ampl_AutoOrMixerDescription = value; OnPropertyChanged("Ampl_AutoOrMixerDescription"); } } }
        public bool Ampl_IsAutoAndMixer { 
            get { return _ampl_IsAutoAndMixer; } 
            set { if (value != _ampl_IsAutoAndMixer) {         
                Ampl_AutoOrMixerDescription = (value == true) ? "Using Auto" : "Using RF"; //description for label for checkbox
                if (_blnAllowSend) { if (value == true) { Automatisation.Commands.Spectrum.SetAmplAutoAttOn(_spectrum); } else { Automatisation.Commands.Spectrum.SetAmplAutoAttOff(_spectrum); } }
                _ampl_IsAutoAndMixer = value;  OnPropertyChanged("Ampl_IsAutoAndMixer");}}}
        public string Ampl_RFAtt { 
            get { return _ampl_RFAtt; } 
            set { if (value != _ampl_RFAtt) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetAmplRefAtt(_spectrum, value); } 
                _ampl_RFAtt = value; OnPropertyChanged("Ampl_RFAtt"); } } }
        public string Ampl_MixLvl { 
            get { return _ampl_MixLvl; } 
            set { if (value != _ampl_MixLvl) {
                //if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetAmpMixerLvl(_spectrum, value); } 
                _ampl_MixLvl = value; OnPropertyChanged("Ampl_MixLvl"); } } }
        public string Ampl_VertScale { 
            get { return _ampl_VertScale; } 
            set { if (value != _ampl_VertScale) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetAmpVertScale(_spectrum, value); } 
                _ampl_VertScale = value; OnPropertyChanged("Ampl_VertScale"); } } }

        //RBW (rbw/fft)
        public string RBWOrFFT
        {
            get { return _rbwOrFFT; }
            set { if (value != _rbwOrFFT) { UpdateRBWFFT(value); _rbwOrFFT = value; OnPropertyChanged("RBWOrFFT"); } }
        }
        public bool RBWEnable
        {
            get { return _rbwMan; }
            set { if (value != _rbwMan) { _rbwMan = value; OnPropertyChanged("RBWEnable"); } }
        }
        public bool FFTEnable
        {
            get { return _fftMan; }
            set { if (value != _fftMan) { _fftMan = value; OnPropertyChanged("FFTEnable"); } }
        }
        public string RBW_Hz { 
            get { return _rbw_Hz; } 
            set { if (value != _rbw_Hz) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetRBWMan(_spectrum, value); } 
                _rbw_Hz = value; OnPropertyChanged("RBW_Hz"); } } }
        public string RBW_FilterShape { 
            get { return _rbw_FilterShape; } 
            set { if (value != _rbw_FilterShape) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetRBWFilter(_spectrum, value); } 
                _rbw_FilterShape = value; OnPropertyChanged("RBW_FilterShape"); } } }
        
        //fft (rbw/fft)
        public string FFT_Points { 
            get { return _fft_Points; } 
            set { if (value != _fft_Points) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFFTPoints(_spectrum, value); }
                _fft_Points = value; OnPropertyChanged("FFT_Points"); } } }
        public string FFT_Windows { 
            get { return _fft_Windows; } 
            set { if (value != _fft_Windows) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetFFTWindowType(_spectrum, value); }
                _fft_Windows = value; OnPropertyChanged("FFT_Windows"); } } }
        public string Extended_Res { get { return _extended_Res; } set { _extended_Res = (int.Parse(value) == 0 || value == "OFF") ? "OFF" : "ON"; { _extended_Res = value; OnPropertyChanged("Extended_Res"); } } }

        //Trace Trace/AVG
        public string TRC { get { return _trc; } set { if (value != _trc) { _trc = value; OnPropertyChanged("TRC"); } } }
        public string TRC_NumberOfTraces { get { return _trc_NumberOfTraces; } set { if (value != _trc_NumberOfTraces) { _trc_NumberOfTraces = value; OnPropertyChanged("TRC_NumberOfTraces"); } } }
        public string TRC_DisplayDetection { get { return _trc_DisplayDetection; } set { if (value != _trc_DisplayDetection) { _trc_DisplayDetection = value; OnPropertyChanged("TRC_DisplayDetection"); } } }

        //Measure
        public string Measure { 
            get { return _measure; } 
            set { if (value != _measure) {
                if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetMeas(_spectrum, value);}
                UpdateMeasure(value);
                _measure = value; OnPropertyChanged("Measure"); } } }

        public string Meas_CHP_Band
        {
            get { return _meas_CHPower_Band; }
            set { if (value != _meas_CHPower_Band) {
                    //CHPower Bandwidth Measurement can only be set when Measure : CHP
                    if (_measure == "CHP"){
                        if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetChannelBand(_spectrum, value); }
                        _meas_CHPower_Band = value; OnPropertyChanged("Meas_CHP_Band"); } } } }

        public string Meas_CHP_Filter { 
            get { return _meas_CHPower_FilterShape; } 
            set { if (value != _meas_CHPower_FilterShape) {
                //CHPower Filter Measurement can only be set when Measure : CHP
                if (_measure == "CHP") {
                    if (_blnAllowSend) { Automatisation.Commands.Spectrum.SetCHPFilterType(_spectrum, value); } 
                    _meas_CHPower_FilterShape = value; OnPropertyChanged("Meas_CHP_Filter"); } } } }

        public string Meas_CHP_RollOff { 
            get { return _meas_CHPower_RollOff; } 
            set { if (value != _meas_CHPower_RollOff) {
                //CHPower Rolloff Measurement can only be set when Measure : CHP
                if (_measure == "CHP") {
                    if (_blnAllowSend) { if (Meas_CHP_Filter == "NYQ" || Meas_CHP_Filter == "RNYQ") { Automatisation.Commands.Spectrum.SetCHPRollOff(_spectrum, value); } } 
                    _meas_CHPower_RollOff = value; OnPropertyChanged("Meas_CHP_RollOff"); } } } }


        #endregion properties

        #region constructor
        public SettingsSpectrumModel(SpectrumMeterModel spectrum)
        {
            _spectrum = spectrum;
            AllowSend = false; //don't allow to send commands for writing settings to analyzer when they have just been set
        }
        #endregion constructor

        #region events
        
        #endregion events

        /// <summary>
        /// When a new table has been selected, update the frequencies
        /// </summary>
        private void ChannelTableChanged()
        {
            if (_blnAllowSend) 
            {
                this.FQ_Center = Automatisation.Commands.Spectrum.GetFreqCenter(_spectrum);
                this.FQ_Start = Automatisation.Commands.Spectrum.GetFreqStart(_spectrum);
                this.FQ_Stop = Automatisation.Commands.Spectrum.GetFreqStop(_spectrum);
            }
        }

        /// <summary>
        /// Update RBW, filtershape, extended res, fft points, windows according to fft or rbw being set to auto
        /// When FFT is set to auto, state and auto can be set or querried
        /// When FFT is set to manual, state can be set and querries, rbw auto cannot
        /// </summary>
        private void UpdateRBWFFT(string value)
        {
            switch (value)
            {
                case "Auto":
                    Automatisation.Commands.Spectrum.SetRBWStateOn(_spectrum); //turn fft to auto first, if fft is set to manual, crash happens when you try to set / get auto
                    Automatisation.Commands.Spectrum.SetRBWAutoOn(_spectrum);
                    RBW_Hz = Automatisation.Commands.Spectrum.GetRBWMan(_spectrum);
                    RBWEnable = false;
                    FFTEnable = false;
                    break;
                case "Man":
                    Automatisation.Commands.Spectrum.SetRBWStateOn(_spectrum); //turn fft to auto first, if fft is set to manual, crash happens when you try to set / get auto
                    Automatisation.Commands.Spectrum.SetRBWAutoOff(_spectrum);
                    RBW_Hz = Automatisation.Commands.Spectrum.GetRBWMan(_spectrum);
                    RBW_FilterShape = Automatisation.Commands.Spectrum.GetRBWFilter(_spectrum);
                    Extended_Res = Commands.Spectrum.GetExtended(_spectrum);
                    RBWEnable = true;
                    FFTEnable = false;
                    break;
                case "FFT":
                    Automatisation.Commands.Spectrum.SetRBWStateOff(_spectrum); 
                    Automatisation.Commands.Spectrum.SetRBWAutoOff(_spectrum);
                    Automatisation.Commands.Spectrum.SetRBWStateOff(_spectrum); //Set state off as last, else auto will change state settings again, and can't get window type
                    FFT_Points = Automatisation.Commands.Spectrum.GetFFTPoints(_spectrum);
                    FFT_Windows = Automatisation.Commands.Spectrum.GetFFTWindowType(_spectrum);
                    Extended_Res = Commands.Spectrum.GetExtended(_spectrum);
                    RBWEnable = false;
                    FFTEnable = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Call when a new measure has been set, only OFF and CHP supported at this time
        /// </summary>
        /// <param name="meas"></param>
        private void UpdateMeasure(string meas)
        {
            switch (meas)
            {
                case "OFF":
                    break;
                case "CHP":
                    this.Meas_CHP_Band = Commands.Spectrum.GetChannelBand(_spectrum);
                    this.Meas_CHP_Filter = Commands.Spectrum.GetCHPFilterType(_spectrum);
                    if(_meas_CHPower_FilterShape == "NYQ" || _meas_CHPower_FilterShape == "RNYQ")
                        this.Meas_CHP_RollOff = Commands.Spectrum.GetCHPRollOff(_spectrum);
                    else
                        this.Meas_CHP_RollOff = "Filter must be (R)NYQ";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get every setting on startup
        /// </summary>
        public void FillInSettings()
        {
            #region FreqChan
            FQ_Center = Commands.Spectrum.GetFreqCenter(_spectrum);
            FQ_Start = Commands.Spectrum.GetFreqStart(_spectrum);
            FQ_Stop = Commands.Spectrum.GetFreqStop(_spectrum);
            ChTable = Commands.Spectrum.GetFreqChanCatAll(_spectrum);
            //if there is no channel table list, don't bother looking for a selected table
            if (ChTable != null)
                ChTableSelected = Commands.Spectrum.GetFreqChanCatSelected(_spectrum).ToUpper();
            //If there is no list selected, don't look for a channel. NONE = no channel list selected    
            if (ChTableSelected.Contains("\"NONE\"") == false && ChTableSelected != null)
                    Ch = Commands.Spectrum.GetFreqChan(_spectrum);
            FQ_Step = Commands.Spectrum.GetFreqCenterStepAutoIncr(_spectrum);
            #endregion FreqChan 
            /* Trigger */

            /* Span */
            Span = Commands.Spectrum.GetFreqSpan(_spectrum);

            #region Ampl
            /* Amplitude */
            Ampl_RefLevel = Commands.Spectrum.GetAmplRefLvl(_spectrum);
            Ampl_IsAutoAndMixer = (Commands.Spectrum.GetAmplAutoAtt(_spectrum).Contains("1")) ? true : false;
            Ampl_RFAtt = Commands.Spectrum.GetAmplRefAtt(_spectrum);
            //command only works when input:attenuation:auto is on
            if (Ampl_IsAutoAndMixer == true) { Ampl_MixLvl = Commands.Spectrum.GetAmpMixerLvl(_spectrum); }
            Ampl_VertScale = Commands.Spectrum.GetAmpVertScale(_spectrum);
            #endregion Ampl          

            #region RBWFFT
            //Get RBW Auto and bind to property
            /*          RBWAUTO     STATE
             *AUTO      1           1
             *MAN       0           1
             *FFT       crash       0
             */
            string state = Commands.Spectrum.GetRBWState(_spectrum);
            string auto = "";
            if (state.Contains("0")) { this.RBWOrFFT = "FFT"; }
            else if (state.Contains("1")) 
            {
                auto = Commands.Spectrum.GetRBWAuto(_spectrum);
                if (auto.Contains("0")) { this.RBWOrFFT = "Man"; }
                if (auto.Contains("1")) { this.RBWOrFFT = "Auto";  }
            }
            #endregion RBWFFT

            #region Measure + Settings
            Measure = Commands.Spectrum.GetMeas(_spectrum);
            Meas_CHP_Band = Commands.Spectrum.GetChannelBand(_spectrum);
            Meas_CHP_Filter = Commands.Spectrum.GetCHPFilterType(_spectrum);
            Meas_CHP_RollOff = Commands.Spectrum.GetCHPRollOff(_spectrum);
            #endregion Measure + Settings
        }
    }
}
