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
using System;

namespace Automatisation.Commands
{
    public static class Spectrum
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static string _logTarget = Automatisation.Logging.TargetNames.SPECTRUM;
        #region Commands
        /* Channel */
        /// <summary>
        /// [:SENSe]:FREQuency:CENTer(?) p455
        /// </summary>
        public static string FREQ_CENTER = ":SENS:FREQ:CENT";
        /// <summary>
        /// [:SENSe]:FREQuency:STARt(?) p461
        /// </summary>
        public static string FREQ_START = ":SENS:FREQ:STAR";
        /// <summary>
        /// [:SENSe]:FREQuency:STOP(?) p461
        /// </summary>
        public static string FREQ_STOP = ":SENS:FREQ:STOP";
        /// <summary>
        /// [:SENSe]:FREQuency:CHANnel(?) p458
        /// </summary>
        public static string FREQ_CHAN = ":SENS:FREQ:CHAN";
        /// <summary>
        /// [:SENSe]:FREQuency:CTABle:CATalog? (Query Only) p458
        /// </summary>
        public static string FREQ_CHAN_TAB_CAT = ":SENS:FREQ:CTAB:CAT?";
        /// <summary>
        /// [:SENSe]:FREQuency:CTABle[:SELect](?) p459
        /// </summary>
        public static string FREQ_CHAN_TAB_SEL = ":SENS:FREQ:CTAB:SEL";
        /// <summary>
        /// [:SENSe]:FREQuency:CENTer:STEP:AUTO(?) p456
        /// </summary>
        public static string FREQ_STEP_AUTO = ":SENS:FREQ:CENT:STEP:AUTO";
        /// <summary>
        /// [:SENSe]:FREQuency:CENTer:STEP:AUTO(?) p456
        /// </summary>
        public static string FREQ_STEP_AUTO_ON = ":SENS:FREQ:CENT:STEP:AUTO ON";
        /// <summary>
        /// [:SENSe]:FREQuency:CENTer:STEP:AUTO(?) p456
        /// </summary>
        public static string FREQ_STEP_AUTO_OFF = ":SENS:FREQ:CENT:STEP:AUTO OFF";
        /// <summary>
        /// [:SENSe]:FREQuency:CENTer:STEP[:INCRement](?) p457 
        /// Note: doesn't affect frontpanel
        /// </summary>
        public static string STEP_AUTO_INCR = ":SENS:FREQ:CENT:STEP:INCR";

        /*Span*/
        /// <summary>
        /// [:SENSe]:FREQuency:SPAN(?) p460
        /// </summary>
        public static string SPAN = ":SENS:FREQ:SPAN";

        /* Trigger */
        /// <summary>
        /// :INITiate:CONTinuous (?) p328
        /// Note: NOTE. When the analyzer receives a :FETCh command while operating in the continuous mode, it returns an execution error. 
        /// If you want to run a :FETCh, use the :INITiate[:IMMediate] command.
        /// </summary>
        public static string INIT_CONT = ":INIT:CONT?";
        public static string REPEAT_CONTINUOUS = ":INIT:CONT ON";
        public static string REPEAT_SINGLE = ":INIT:CONT OFF";
        public static string TRIGGER_MODE_AUTO = ":TRIG:SEQ:MODE AUTO";
        public static string TRIGGER_MODE_NORM = ":TRIG:SEQ:MODE NORM";

        /* amplitude */
        /// <summary>
        /// :INPut:MLEVel (?) p336
        /// </summary>
        public static string AMP_REF_LEVEL = ":INP:MLEV";
        /// <summary>
        /// :INPut:ALEVel (No Query Form) p332
        /// </summary>
        public static string AMP_AUTO_LEVEL = ":INP:ALEV";
        /// <summary>
        /// :INPut:ATTenuation:AUTO (?) p333
        /// </summary>
        public static string AMP_ATT_AUTO = ":INP:ATT:AUTO?";
        /// <summary>
        /// :INPut:ATTenuation:AUTO (?) p333
        /// </summary>
        public static string AMP_ATT_AUTO_ON = ":INP:ATT:AUTO ON";
        /// <summary>
        /// :INPut:ATTenuation:AUTO (?) p333
        /// </summary>
        public static string AMP_ATT_AUTO_OFF = ":INP:ATT:AUTO OFF";
        /// <summary>
        /// :INPut:ATTenuation (?) p332
        /// </summary>
        public static string AMP_RF_ATT_DB = ":INP:ATT";
        /// <summary>
        /// :INPut:MIXer (?) p335
        /// </summary>
        public static string AMP_MIX_LVL = ":INP:MIX";
        /// <summary>
        /// :DISPlay:SPECtrum:Y[:SCALe]:PDIVision(?) p220
        /// </summary>
        public static string AMP_VERT_SCALE = ":DISP:SPEC:Y:SCAL:PDIV";

        /*RBW/FFT*/
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth[:RESolution]:AUTO(?) p507
        /// </summary>
        public static string RBW_AUTO = ":SENS:SPEC:BAND:RES:AUTO?";
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth[:RESolution]:AUTO(?) p507
        /// </summary>
        public static string RBW_AUTO_ON = ":SENS:SPEC:BAND:RES:AUTO ON";
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth[:RESolution]:AUTO(?) p507
        /// </summary>
        public static string RBW_AUTO_OFF = ":SENS:SPEC:BAND:RES:AUTO OFF";
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe(?) p508
        /// </summary>
        public static string RBW_STATE = ":SENS:SPEC:BAND:STAT?";
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe(?) p508
        /// </summary>
        public static string RBW_STATE_ON = ":SENS:SPEC:BAND:STAT ON";
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe(?) p508
        /// </summary>
        public static string RBW_STATE_OFF = ":SENS:SPEC:BAND:STAT OFF";
        /// <summary>
        /// [:SENSe]:SPECtrum:BANDwidth|:BWIDth[:RESolution](?) p507
        /// </summary>
        public static string RBW_MAN = ":SENS:SPEC:BAND:RES";
        /// <summary>
        /// [:SENSe]:SPECtrum:FILTer:TYPE(?) p512
        /// </summary>
        public static string RBW_FILTER = ":SENS:SPEC:FILT:TYPE"; //{ RECTangle | GAUSsian | NYQuist | RNYQuist }
        /// <summary>
        /// [:SENSe]:SPECtrum:FFT:LENGth(?) p514
        /// </summary>
        public static string FFT_POINTS = ":SENS:SPEC:FFT:LENG";
        /// <summary>
        /// [:SENSe]:SPECtrum:FFT:WINDow[:TYPE](?) p515
        /// </summary>
        public static string FFT_WINDOW_TYPE = ":SENS:SPEC:FFT:WIND:TYPE";
        /// <summary>
        /// [:SENSe]:SPECtrum:FFT:ERESolution(?) p513
        /// </summary>
        public static string FFT_EXTENDED = ":SENS:SPEC:FFT:ERES?";
        /// <summary>
        /// [:SENSe]:SPECtrum:FFT:ERESolution(?) p513
        /// </summary>
        public static string FFT_EXTENDED_ON = ":SENS:SPEC:FFT:ERES ON";
        /// <summary>
        /// [:SENSe]:SPECtrum:FFT:ERESolution(?) p513
        /// </summary>
        public static string FFT_EXTENDED_OFF = ":SENS:SPEC:FFT:ERES OFF";


        /* TRACE */
        /// <summary>
        /// :TRACe<x>|:DATA<x>:MODE (?) p574
        /// </summary>
        public static string TRACE_1_MODE = ":TRAC1:MODE"; //MODE { NORMal | AVERage | MAXHold | MINHold | FREeze | OFF }
        /// <summary>
        /// :TRACe<x>|:DATA<x>:MODE (?) p574
        /// </summary>
        public static string TRACE_2_MODE = ":TRAC2:MODE"; //MODE { NORMal | AVERage | MAXHold | MINHold | FREeze | OFF }
        /// <summary>
        /// :TRACe<x>|:DATA<x>:AVERage:COUNt (?) p572
        /// </summary>
        public static string TRACE_1_COUNT = "TRAC1:AVER:COUN"; //enkel in AVER, MAXH, MINH
        /// <summary>
        /// :TRACe<x>|:DATA<x>:AVERage:COUNt (?) p572
        /// </summary>
        public static string TRACE_2_COUNT = "TRAC2:AVER:COUN"; //enkel in AVER, MAXH, MINH
        /// <summary>
        /// :TRACe<x>|:DATA<x>:AVERage:CLEar (No Query Form) p572
        /// </summary>
        public static string TRACE_1_CLEAR = "TRAC1:AVER:CLE"; //enkel in AVER, MAXH, MINH
        /// <summary>
        /// :TRACe<x>|:DATA<x>:AVERage:CLEar (No Query Form) p572
        /// </summary>
        public static string TRACE_2_CLEAR = "TRAC2:AVER:CLE"; //enkel in AVER, MAXH, MINH
        //[:SENSe]:SPECtrum:AVERage:TYPE(?) 
        //[:SENSe]:SPECtrum:AVERage[:STATe](?)
        //[:SENSe]:SPECtrum:AVERage:COUNt(?)
        //[:SENSe]:SPECtrum:AVERage:CLEar(NoQueryForm)

        /*Meas*/
        /// <summary>
        /// [:SENSe]:SPECtrum:MEASurement(?) p517
        /// </summary>
        public static string MEAS = ":SENS:SPEC:MEAS";

        /*Meas Setup*/
        /* CHP */
        /// <summary>
        /// [:SENSe]:CHPower:FILTer:COEFficient(?) p420
        /// </summary>
        public static string CHP_ROLLOFF = ":SENS:CHP:FILT:COEF";
        /// <summary>
        /// [:SENSe]:CHPower:FILTer:TYPE(?) p512
        /// </summary>
        public static string CHP_FILTER_TYPE = ":SENS:CHP:FILT:TYPE"; //{ RECTangle | GAUSsian | NYQuist | RNYQuist }
        /// <summary>
        /// [:SENSe]:CHPower:BANDwidth|:BWIDth:INTegration(?) p419
        /// </summary>
        public static string CHP_BANDWIDTH = ":SENS:CHP:BAND:INT";


        /* Other */
        /// <summary>
        /// :INSTrument[:SELect] (?) p339
        /// </summary>
        public static string INSTRUMENT_SELECT = ":INST:SEL";
        /// <summary>
        /// :READ:SPECtrum? (Query Only) p384
        /// </summary>
        public static string READ_SPECTRUM = ":READ:SPEC?";
        /// <summary>
        /// :READ:SPECtrum:CHPower? (Query Only) p386
        /// </summary>
        public static string READ_SPECTRUM_CHP = ":READ:SPEC:CHP?";
        /// <summary>
        /// :FETCh:SPECtrum? (Query Only) p303
        /// </summary>
        public static string FETCH_SPECTRUM = ":FETC:SPEC?";
        /// <summary>
        /// :FETCh:SPECtrum:CHPower? (Query Only) p305
        /// </summary>
        public static string FETCH_SPECTRUM_CHP = "FETC:SPEC:CHP?";
        #endregion Commands







        /* FREQUENCY */
        #region FREQ_CENTER
        /// <summary>
        /// Sets or queries the center frequency.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetFreqCenter(SpectrumMeterModel s)
        {
            string res = string.Empty;
            try { res = s.WriteCommandRes(FREQ_CENTER + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FREQ_CENTER: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the center frequency. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par"><freq>::=<NRf> specifies the center frequency. For the setting range</param>
        public static void SetFreqCenter(SpectrumMeterModel s, string par)
        {
            try { s.WriteCommand(FREQ_CENTER + " " + par + "hz"); }
            catch (Exception e) { Log.Error(_logTarget + "Set FREQ_CENTER: " + e.ToString(), e); }
        }
        #endregion FREQ_CENTER

        #region FREQ_START
        /// <summary>
        /// Sets or queries the start frequency.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Specifies the start frequency. For the setting range, refer to Table 2--52 on page 2--428.</returns>
        public static string GetFreqStart(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FREQ_START + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FQ_Center: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the start frequency.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the start frequency. For the setting range, refer to Table 2--52 on page 2--428.</param>
        public static void SetFreqStart(SpectrumMeterModel s, string par) { s.WriteCommand(FREQ_START + " " + par + "hz"); }
        #endregion FREQ_START

        #region FREQ_STOP
        /// <summary>
        /// Sets or queries the start frequency.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Specifies the stop frequency. For the setting range, refer to Table 2--52 on page 2--428</returns>
        public static string GetFreqStop(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FREQ_STOP + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FREQ_STOP: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the start frequency.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the stop frequency. For the setting range, refer to Table 2--52 on page 2--428</param>
        public static void SetFreqStop(SpectrumMeterModel s, string par) { s.WriteCommand(FREQ_STOP + " " + par + "hz"); }
        #endregion FREQ_STOP

        /* CHANNELS */
        #region ChannelFrequency
        /// <summary>
        /// Sets or queries a channel number in the channel table specified with the [:SENSe]:FREQuency:CTABle[:SELect] command.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies a channel number in the channel table.</returns>
        public static string GetFreqChan(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FREQ_CHAN + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FREQ_CHAN: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries a channel number in the channel table specified with the [:SENSe]:FREQuency:CTABle[:SELect] command.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies a channel number in the channel table.</param>
        public static void SetFreqChan(SpectrumMeterModel s, string par) { s.WriteCommand(FREQ_CHAN + " " + par); }
        #endregion ChannelFrequency

        #region ChannelTable
        /// <summary>
        /// Queries the available channel tables. QUERY ONLY!
        /// </summary>
        /// <param name="s"></param>
        /// <returns>the available channel table name(s). 
        /// If more than one table is available, the table names are separated with comma</returns>
        public static string GetFreqChanCatAll(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FREQ_CHAN_TAB_CAT); }
            catch (Exception e) { Log.Error(_logTarget + "Get FREQ_CHAN_TAB_CAT: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects the channel table. The query command returns the selected channel table.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies a channel table. 
        /// The table name is represented with the communication standard name followed by “-FL” (forward link), “-RL” (reverse link), “-UL” (uplink), or “-DL” (downlink).</returns>
        public static string GetFreqChanCatSelected(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FREQ_CHAN_TAB_SEL + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FREQ_CHAN_TAB_SEL: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects the channel table. The query command returns the selected channel table.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies a channel table. 
        /// The table name is represented with the communication standard name followed by “-FL” (forward link), “-RL” (reverse link), “-UL” (uplink), or “-DL” (downlink).</param>
        public static void SetFreqChanCat(SpectrumMeterModel s, string par) { s.WriteCommand(FREQ_CHAN_TAB_SEL + " " + par); }
        #endregion ChannelTable

        #region FrequencyAutoStep
        /// <summary>
        /// Determines whether to automatically set the step size (amount per click by which the up and down keys change a setting value) of the center frequency by the span setting.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>OFF or 0 specifies that the step size of the center frequency is not set automatically. 
        /// To set it, use the [:SENSe]:FREQuency:CENTer:STEP[:INCRement] command. 
        /// ON or 1 specifies that the step size of the center frequency is set automatically by the span.</returns>
        public static string GetFreqCenterStepAuto(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FREQ_STEP_AUTO + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FREQ_STEP_AUTO: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Determines whether to automatically set the step size (amount per click by which the up and down keys change a setting value) of the center frequency by the span setting.
        /// OFF or 0 specifies that the step size of the center frequency is not set automatically. 
        /// To set it, use the [:SENSe]:FREQuency:CENTer:STEP[:INCRement] command. 
        /// ON or 1 specifies that the step size of the center frequency is set automatically by the span.
        /// </summary>
        /// <param name="s"></param>
        public static void SetFreqCenterStepAutoOn(SpectrumMeterModel s) { s.WriteCommand(FREQ_STEP_AUTO_ON); }
        /// <summary>
        /// Determines whether to automatically set the step size (amount per click by which the up and down keys change a setting value) of the center frequency by the span setting.
        /// OFF or 0 specifies that the step size of the center frequency is not set automatically. 
        /// To set it, use the [:SENSe]:FREQuency:CENTer:STEP[:INCRement] command. 
        /// ON or 1 specifies that the step size of the center frequency is set automatically by the span.
        /// </summary>
        /// <param name="s"></param>
        public static void SetFreqCenterStepAutoOff(SpectrumMeterModel s) { s.WriteCommand(FREQ_STEP_AUTO_OFF); }
        #endregion FrequencyAutoStep

        #region FrequencyStep
        /// <summary>
        /// Sets or queries the step size (amount per click by which the up and down keys change a setting value) of the center frequency when [:SENSe]:FREQuency:CENTer:STEP:AUTO is OFF.
        /// Note: doesn't affect frontpanel
        /// </summary>
        /// <param name="s"></param>
        /// <returns>the step size of the center frequency</returns>
        public static string GetFreqCenterStepAutoIncr(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(STEP_AUTO_INCR + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get STEP_AUTO_INCR: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the step size (amount per click by which the up and down keys change a setting value) of the center frequency when [:SENSe]:FREQuency:CENTer:STEP:AUTO is OFF.
        /// Note: doesn't affect frontpanel
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">the step size of the center frequency</param>
        public static void SetFreqCenterStepAutoIncr(SpectrumMeterModel s, string par) { s.WriteCommand(STEP_AUTO_INCR + " " + par); }
        #endregion FrequencyStep

        /* SPAN */
        #region SpanFrequency
        /// <summary>
        /// Sets or queries the span.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the span. The valid range depends on the measurement mode as listed in Table 2--53</returns>
        public static string GetFreqSpan(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(SPAN + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get SPAN: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the span.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the span. The valid range depends on the measurement mode as listed in Table 2--53</param>
        public static void SetFreqSpan(SpectrumMeterModel s, string par) { s.WriteCommand(SPAN + " " + par + "hz"); }
        #endregion SpanFrequency
        //missing start & stop span frequency - same as freq_start en stop from frequency

        /* TRIGGER */
        #region init|repeat continuous / single
        /// <summary>
        /// Determines whether to use the continuous mode to acquire the input signal
        /// OFF or 0 specifies that the single mode, rather than the continuous mode, is used for data acquisition. To initiate the acquisition, use the :INITiate[:IMMediate]
        /// To stop the acquisition because the trigger is not generated in single mode, send the following command: :INITiate:CONTinuous OFF
        /// ON or 1 initiates data acquisition in the continuous mode.
        /// To stop the acquisition in the continuous mode, send the following command::INITiate:CONTinuous OFF
        /// NOTE. When the analyzer receives a :FETCh command while operating in the continuous mode, it returns an execution error. If you want to run a :FETCh, use the :INITiate[:IMMediate] command.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>OFF or 0 = single mode, ON or 1 = continuous mode</returns>
        public static string GetInitCont(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(INIT_CONT + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get INIT_CONT: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Determines whether to use the continuous mode to acquire the input signal
        /// OFF or 0 specifies that the single mode, rather than the continuous mode, is used for data acquisition. To initiate the acquisition, use the :INITiate[:IMMediate]
        /// To stop the acquisition because the trigger is not generated in single mode, send the following command: :INITiate:CONTinuous OFF
        /// ON or 1 initiates data acquisition in the continuous mode.
        /// To stop the acquisition in the continuous mode, send the following command::INITiate:CONTinuous OFF
        /// NOTE: When the analyzer receives a :FETCh command while operating in the continuous mode, it returns an execution error. 
        /// If you want to run a :FETCh, use the :INITiate[:IMMediate] command.
        /// </summary>
        /// <param name="s"></param>
        public static void SetInitContOn(SpectrumMeterModel s) { s.WriteCommand(REPEAT_CONTINUOUS); }
        /// <summary>
        /// Determines whether to use the continuous mode to acquire the input signal
        /// OFF or 0 specifies that the single mode, rather than the continuous mode, is used for data acquisition. To initiate the acquisition, use the :INITiate[:IMMediate]
        /// To stop the acquisition because the trigger is not generated in single mode, send the following command: :INITiate:CONTinuous OFF
        /// ON or 1 initiates data acquisition in the continuous mode.
        /// To stop the acquisition in the continuous mode, send the following command::INITiate:CONTinuous OFF
        /// </summary>
        /// <param name="s"></param>
        public static void SetInitContOff(SpectrumMeterModel s) { s.WriteCommand(REPEAT_SINGLE); }
        #endregion init|repeat continuous / single
        #region TriggerMode
        #endregion TriggerMode

        /* AMPLITUDE */
        #region AmplitudeReferenceLevel
        /// <summary>
        /// Sets or queries the reference level. Using this command to set the reference level is equivalent to pressing the AMPLITUDE key and then the Ref Level side key on the front panel.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the reference level. The valid settings depend on the measurement frequency band as shown in Table 2--43</returns>
        public static string GetAmplRefLvl(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(AMP_REF_LEVEL + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get AMP_REF_LEVEL: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the reference level. Using this command to set the reference level is equivalent to pressing the AMPLITUDE key and then the Ref Level side key on the front panel.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the reference level. The valid settings depend on the measurement frequency band as shown in Table 2--43</param>
        public static void SetAmplRefLvl(SpectrumMeterModel s, string par) { s.WriteCommand(AMP_REF_LEVEL + " " + par); }
        #endregion AmplitudeReferenceLevel

        #region AmplitudeAutoLevel
        /// <summary>
        /// Adjusts amplitude automatically for the best system performance using the input signal as a guide.
        /// </summary>
        /// <param name="s"></param>
        public static void SetAmplAutoLvl(SpectrumMeterModel s) { s.WriteCommand(AMP_REF_LEVEL); }
        #endregion AmplitudeAutoLevel
        #region Amplitude Auto|RF|Mixer
        #region AmplitudeAttenuationAuto
        /// <summary>
        /// Determines whether to automatically set the input attenuation according to the reference level
        /// </summary>
        /// <param name="s"></param>
        /// <returns>OFF or 0 specifies that the input attenuation is not set automatically. To set it, use the :INPut:ATTenuation command. ON or 1 specifies that the input attenuation is set automatically.</returns>
        public static string GetAmplAutoAtt(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(AMP_ATT_AUTO); }
            catch (Exception e) { Log.Error(_logTarget + "Get AMP_ATT_AUTO: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Determines whether to automatically set the input attenuation according to the reference level
        /// ON or 1 specifies that the input attenuation is set automatically.
        /// </summary>
        /// <param name="s"></param>
        public static void SetAmplAutoAttOn(SpectrumMeterModel s) { s.WriteCommand(AMP_ATT_AUTO_ON); }
        /// <summary>
        /// Determines whether to automatically set the input attenuation according to the reference level
        /// OFF or 0 specifies that the input attenuation is not set automatically. To set it, use the :INPut:ATTenuation command.
        /// </summary>
        /// <param name="s"></param>
        public static void SetAmplAutoAttOff(SpectrumMeterModel s) { s.WriteCommand(AMP_ATT_AUTO_OFF); }
        #endregion AmplitudeAttenuationAuto
        #region AmplitudeReferenceAttenuation
        /// <summary>
        /// When you have selected OFF or 0 in the :INPut:ATTenuation:AUTO command, use this command to set the input attenuation. The query version of this command returns the input attenuation setting.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the input attenuation. The valid settings depend on the measurement frequency band as shown in Table 2--41.</returns>
        public static string GetAmplRefAtt(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(AMP_RF_ATT_DB + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get AMP_RF_ATT_DB: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// When you have selected OFF or 0 in the :INPut:ATTenuation:AUTO command, use this command to set the input attenuation. The query version of this command returns the input attenuation setting.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the input attenuation. The valid settings depend on the measurement frequency band as shown in Table 2--41.</param>
        public static void SetAmplRefAtt(SpectrumMeterModel s, string par) { s.WriteCommand(AMP_RF_ATT_DB + " " + par); }
        #endregion AmplitudeReferenceAttenuation
        #region AmplitudeMixerLevel
        /// <summary>
        /// Selects or queries the mixer level.
        /// NOTE. To set the mixer level, you must have selected On in the :INPut:ATTenuation:AUTO command.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the mixer level. The valid settings depend on the measurement frequency band as shown in Table 2--42.</returns>
        public static string GetAmpMixerLvl(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(AMP_MIX_LVL + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get AMP_MIX_LVL: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects or queries the mixer level.
        /// To set the mixer level, you must have selected On in the :INPut:ATTenuation:AUTO command.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the mixer level. The valid settings depend on the measurement frequency band as shown in Table 2--42.</param>
        public static void SetAmpMixerLvl(SpectrumMeterModel s, string par) { s.WriteCommand(AMP_MIX_LVL + " " + par); }
        #endregion AmplitudeMixerLevel
        #endregion Amplitude Auto|RF|Mixer

        #region AmplitudeVerticalScale
        /// <summary>
        /// Sets or queries the vertical, or amplitude, scale (per division) in the spectrum view.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the horizontal scale in the spectrum view. Range: 0 to 10 dB/div.</returns>
        public static string GetAmpVertScale(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(AMP_VERT_SCALE + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get AMP_VERT_SCALE: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the vertical, or amplitude, scale (per division) in the spectrum view.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the horizontal scale in the spectrum view. Range: 0 to 10 dB/div.</param>
        public static void SetAmpVertScale(SpectrumMeterModel s, string par) { s.WriteCommand(AMP_VERT_SCALE + " " + par); }
        #endregion AmplitudeVerticalScale

        /* RBW / FFT */
        #region RBWAuto
        /// <summary>
        /// Determines whether to automatically set the resolution bandwidth (RBW) by the span setting.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>OFF or 0 specifies that the RBW is not set automatically. 
        /// To set it, use the [:SENSe]:SPECtrum:BANDwidth|:BWIDth[:RESolution] command.
        /// ON or 1 specifies that the RBW is set automatically.</returns>
        public static string GetRBWAuto(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(RBW_AUTO); }
            catch (Exception e) { Log.Error(_logTarget + "Get RBW_AUTO: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Determines whether to automatically set the resolution bandwidth (RBW) by the span setting.
        /// ON or 1 specifies that the RBW is set automatically.
        /// </summary>
        /// <param name="s"></param>
        public static void SetRBWAutoOn(SpectrumMeterModel s) { s.WriteCommand(RBW_AUTO_ON); }
        /// <summary>
        /// Determines whether to automatically set the resolution bandwidth (RBW) by the span setting.
        /// OFF or 0 specifies that the RBW is not set automatically. To set it, use the [:SENSe]:SPECtrum:BANDwidth|:BWIDth[:RESolution] command
        /// </summary>
        /// <param name="s"></param>
        public static void SetRBWAutoOff(SpectrumMeterModel s) { s.WriteCommand(RBW_AUTO_OFF); }
        #endregion RBWAuto
        #region SpectrumBandStateForFFT
        /// <summary>
        /// Determines whether to perform the resolution bandwidth (RBW) process
        /// </summary>
        /// <param name="s"></param>
        /// <returns>OFF or 0 specifies that the RBW process is not performed so that a spectrum immediately after the FFT process is displayed on screen.
        /// ON or 1 specifies that the RBW process is performed.</returns>
        public static string GetRBWState(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(RBW_STATE); }
            catch (Exception e) { Log.Error(_logTarget + "Get RBW_STATE: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Determines whether to perform the resolution bandwidth (RBW) process
        /// ON or 1 specifies that the RBW process is performed.
        /// </summary>
        /// <param name="s"></param>
        public static void SetRBWStateOn(SpectrumMeterModel s) { s.WriteCommand(RBW_STATE_ON); }
        /// <summary>
        /// Determines whether to perform the resolution bandwidth (RBW) process
        /// OFF or 0 specifies that the RBW process is not performed so that a spectrum immediately after the FFT process is displayed on screen.
        /// </summary>
        /// <param name="s"></param>
        public static void SetRBWStateOff(SpectrumMeterModel s) { s.WriteCommand(RBW_STATE_OFF); }
        #endregion SpectrumBandStateForFFT
        #region RBWMan
        /// <summary>
        /// Sets or queries the resolution bandwidth (RBW) when [:SENSe]:SPECtrum: BANDwidth|:BWIDth[:RESolution]:AUTO is set to Off.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the RBW. For the setting range, refer to Table D--4 in Appendix D.</returns>
        public static string GetRBWMan(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(RBW_MAN + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get RBW_MAN: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the resolution bandwidth (RBW) when [:SENSe]:SPECtrum: BANDwidth|:BWIDth[:RESolution]:AUTO is set to Off.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the RBW. For the setting range, refer to Table D--4 in Appendix D.</param>
        public static void SetRBWMan(SpectrumMeterModel s, string par) { s.WriteCommand(RBW_MAN + " " + par + "Hz"); }
        #endregion RBWMan
        #region RbwFilter
        /// <summary>
        /// Selects or queries the RBW filter.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>RECTangle selects the rectangular filter. 
        /// GAUSsian selects the Gaussian filter.
        /// NYQuist selects the Nyquist filter (default).
        /// RNYQuist selects the Root Nyquist filter.</returns>
        public static string GetRBWFilter(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(RBW_FILTER + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get RBW_FILTER: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects or queries the RBW filter.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">RECTangle selects the rectangular filter. 
        /// GAUSsian selects the Gaussian filter.
        /// NYQuist selects the Nyquist filter (default).
        /// RNYQuist selects the Root Nyquist filter.</param>
        public static void SetRBWFilter(SpectrumMeterModel s, string par) { s.WriteCommand(RBW_FILTER + " " + par); }
        #endregion RbwFilter
        #region FFTPoints
        /// <summary>
        /// Sets or queries the number of FFT points. This command is valid when [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe is OFF.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>sets the number of FFT points. Range: 64 to 65536 in powers of 2.</returns>
        public static string GetFFTPoints(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FFT_POINTS + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FFT_POINTS: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the number of FFT points. This command is valid when [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe is OFF.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">sets the number of FFT points. Range: 64 to 65536 in powers of 2.</param>
        public static void SetFFTPoints(SpectrumMeterModel s, string par) { s.WriteCommand(FFT_POINTS + " " + par); }
        #endregion FFTPoints
        #region FFTWindowType
        //{ BH3A | BH3B | BH4A | BH4B | BLACkman | HAMMing | HANNing | PARZen | ROSenfield | WELCh | SLOBe | SCUBed | ST4T | FLATtop | RECT }
        /// <summary>
        /// Selects or queries the FFT window function. This command is valid when [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe is OFF.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>{ BH3A | BH3B | BH4A | BH4B | BLACkman | HAMMing | HANNing | PARZen | ROSenfield | WELCh | SLOBe | SCUBed | ST4T | FLATtop | RECT }</returns>
        public static string GetFFTWindowType(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FFT_WINDOW_TYPE + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get FFT_WINDOW_TYPE: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects or queries the FFT window function. This command is valid when [:SENSe]:SPECtrum:BANDwidth|:BWIDth:STATe is OFF.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">{ BH3A | BH3B | BH4A | BH4B | BLACkman | HAMMing | HANNing | PARZen | ROSenfield | WELCh | SLOBe | SCUBed | ST4T | FLATtop | RECT }</param>
        public static void SetFFTWindowType(SpectrumMeterModel s, string par) { s.WriteCommand(FFT_WINDOW_TYPE + " " + par); }
        #endregion FFTWindowType
        #region RBWFFTExtended
        /// <summary>
        /// Determines whether to enable the extended resolution that eliminates the limit on the number of FFT points (it is normally limited internally).
        /// ON or 1 allows you to set the number of FFT points up to 65536. Use the [:SENSe]:SPECtrum:FFT:LENGth command to set the number.
        /// OFF or 0 disables the extended resolution. The number of FFT points is limited internally.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetExtended(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FFT_EXTENDED); }
            catch (Exception e) { Log.Error(_logTarget + "Get FFT_EXTENDED: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Determines whether to enable the extended resolution that eliminates the limit on the number of FFT points (it is normally limited internally).
        /// ON or 1 allows you to set the number of FFT points up to 65536. Use the [:SENSe]:SPECtrum:FFT:LENGth command to set the number.
        /// </summary>
        /// <param name="s"></param>
        public static void SetExtendedOn(SpectrumMeterModel s) { s.WriteCommand(FFT_EXTENDED_ON); }
        /// <summary>
        /// Determines whether to enable the extended resolution that eliminates the limit on the number of FFT points (it is normally limited internally).
        /// OFF or 0 disables the extended resolution. The number of FFT points is limited internally.
        /// </summary>
        /// <param name="s"></param>
        public static void SetExtendedOff(SpectrumMeterModel s) { s.WriteCommand(FFT_EXTENDED_OFF); }
        #endregion RBWFFTExtended

        /* trace */

        /*Measurement */
        #region MEAS
        /// <summary>
        /// Selects and runs the measurement item in the S/A (spectrum analysis) mode. The query version of this command returns the current measurement item.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetMeas(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(MEAS + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get MEAS: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects and runs the measurement item in the S/A (spectrum analysis) mode. Thequery version of this command returns the current measurement item.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par"></param>
        public static void SetMeas(SpectrumMeterModel s, string par) { s.WriteCommand(MEAS + " " + par); }
        #endregion MEAS

        /* Meas Setup */
        #region CHANNELPOWERBANDWIDTH
        /// <summary>
        /// Sets or queries the channel bandwidth for the channel power measurement (seeFigure 2--18).
        /// </summary>
        /// <param name="s"></param>
        /// <returns>specifies the channel bandwidth for the channel power measurement. Range: (Bin bandwidth)×8 to full span [Hz]</returns>
        public static string GetChannelBand(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(CHP_BANDWIDTH + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get CHP_BANDWIDTH: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Sets or queries the channel bandwidth for the channel power measurement (seeFigure 2--18).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">specifies the channel bandwidth for the channel power measurement. Range: (Bin bandwidth)×8 to full span [Hz]</param>
        public static void SetChannelBand(SpectrumMeterModel s, string par) { s.WriteCommand(CHP_BANDWIDTH + " " + par + "hz"); }
        #endregion CHANNELPOWERBANDWIDTH
        #region CHANNELPOWERFILTER
        /// <summary>
        /// Selects or queries the filter for the channel power measurement
        /// </summary>
        /// <param name="s"></param>
        /// <returns>RECTangle selects the rectangular filter. 
        /// GAUSsian selects the Gaussian filter.
        /// NYQuist selects the Nyquist filter (default).
        /// RNYQuist selects the Root Nyquist filter.</returns>
        public static string GetCHPFilterType(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(CHP_FILTER_TYPE + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get CHP_FILTER_TYPE: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects or queries the filter for the channel power measurement
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">RECTangle selects the rectangular filter. 
        /// GAUSsian selects the Gaussian filter.
        /// NYQuist selects the Nyquist filter (default).
        /// RNYQuist selects the Root Nyquist filter.</param>
        public static void SetCHPFilterType(SpectrumMeterModel s, string par) { s.WriteCommand(CHP_FILTER_TYPE + " " + par); }
        #endregion RbwFilter
        #region CHANNELPOWERROLLOFF
        /// <summary>
        /// Selects or queries the filter for the channel power measurement
        /// Sets or queries the roll-off rate of the filter for the channel power measurement when you have selected either NYQuist (Nyquist filter) or RNYQuist (Root Nyquist filter) in the [:SENSe]:CHPower:FILTer:TYPE command
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Range: 0.0001 to 1</returns>
        public static string GetCHPRollOff(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(CHP_ROLLOFF + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get CHP_ROLLOFF: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects or queries the filter for the channel power measurement
        /// Sets or queries the roll-off rate of the filter for the channel power measurement when you have selected either NYQuist (Nyquist filter) or RNYQuist (Root Nyquist filter) in the [:SENSe]:CHPower:FILTer:TYPE command
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">Range: 0.0001 to 1</param>
        public static void SetCHPRollOff(SpectrumMeterModel s, string par) { s.WriteCommand(CHP_ROLLOFF + " " + par); }
        #endregion CHANNELPOWERROLLOFF

        #region other
        /*
         The :FETCh commands retrieve the measurements from the data taken by the latest INITiate command.
         If you want to perform a FETCh operation on fresh data, use the :READ commands, which acquire a new input signal and fetch the measurement results from that data.
         */

        /* Other */
        #region INSTRUMENTSELECT
        /// <summary>
        /// Selects or queries the measurement mode
        /// NOTE. If you want to change the measurement mode, stop the data acquisition with the :INITiate:CONTinuous OFF command.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetInstrumentSelect(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(INSTRUMENT_SELECT + "?"); }
            catch (Exception e) { Log.Error(_logTarget + "Get INSTRUMENT_SELECT: " + e.ToString(), e); }
            return res;
        }
        /// <summary>
        /// Selects or queries the measurement mode
        /// NOTE. If you want to change the measurement mode, stop the data acquisition with the :INITiate:CONTinuous OFF command.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="par">measurement mode</param>
        public static void SetInstrumentSelect(SpectrumMeterModel s, string par) { s.WriteCommand(INSTRUMENT_SELECT + " " + par); }
        #endregion INSTRUMENTSELECT

        /// <summary>
        /// Obtains spectrum waveform data in the S/A (spectrum analysis) mode.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>#Num_digit Num_byte Data(1)Data(2)...Data(n)
        /// Where
        /// Num_digit is the number of digits in <Num_byte>.
        /// Num_byte is the number of bytes of the data that follow.
        /// Data(n) is the amplitude spectrum in dBm.
        /// 4-byte little endian floating-point format specified in IEEE 488.2
        /// n: Max 240001</returns>
        public static string ReadSpectrum(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(READ_SPECTRUM); }
            catch (Exception e) { Log.Error(_logTarget + "Get READ_SPECTRUM: " + e.ToString(), e); }
            return res;
        }

        /// <summary>
        /// Obtains the results of the carrier frequency measurement in the S/A mode.
        /// </summary>
        /// <param name="s"></param>
        /// <returns><cfreq>::=<NRf> is the measured value of the carrier frequency in Hz.</returns>
        public static string ReadSpectrumCHP(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(READ_SPECTRUM_CHP); }
            catch (Exception e) { Log.Error(_logTarget + "Get READ_SPECTRUM_CHP: " + e.ToString(), e); }
            return res;
        }

        /// <summary>
        /// Returns spectrum waveform data in the S/A (spectrum analysis) mode.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>#Num_digit Num_byte Data(1)Data(2)...Data(n)
        /// Where
        /// Num_digit is the number of digits in <Num_byte>.
        /// Num_byte is the number of bytes of the data that follow.
        /// Data(n) is the amplitude spectrum in dBm.
        /// 4-byte little endian floating-point format specified in IEEE 488.2
        /// n: Max 240001</returns>
        public static string FetchSpectrum(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { s.WriteCommand(FETCH_SPECTRUM); }
            catch (Exception e) { Log.Error(_logTarget + "Get FETCH_SPECTRUM: " + e.ToString(), e); }
            return res;
        }

        /// <summary>
        /// Returns the results of the channel power measurement in the S/A (spectrum analysis) mode.
        /// </summary>
        /// <param name="s"></param>
        /// <returns><chpower>::=<NRf> is the channel power measured value in dBm.</returns>
        public static string FetchSpectrumCHP(SpectrumMeterModel s)
        {
            string res = String.Empty;
            try { res = s.WriteCommandRes(FETCH_SPECTRUM_CHP); }
            catch (Exception e) { Log.Error(_logTarget + "Get FETCH_SPECTRUM_CHP: " + e.ToString(), e); }
            return res;
        }
        #endregion other
    }
}
