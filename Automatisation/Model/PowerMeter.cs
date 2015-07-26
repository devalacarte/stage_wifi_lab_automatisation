/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using Automatisation.Commands;
using System;

namespace Automatisation.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class PowerMeterModel : MetersModel
    {


        #region Fields
        private string _channelStatusDesc;
        private static string COND_NO_SENSOR = "Not connected";
        private static string COND_CONNECTED_A = "A connected";
        private static string COND_CONNECTED_B = "B connected";
        private static string COND_CONNECTED_A_B = "A and B connected";
        private static string COND_ERROR_A = "Error A";
        private static string COND_ERROR_B = "Error B";
        #endregion Fields

        #region properties
        /// <summary>
        /// Channelstatus can have 6 different values
        /// 0 = no sensor
        /// 2 = a connected
        /// 4 = b connected
        /// 6 = a and b are connected
        /// 8 = error for channel a
        /// 16 = error for channel b
        /// </summary>
        public string ChannelStatusDesc
        {
            get { return _channelStatusDesc; }
            set { if (value != _channelStatusDesc) { _channelStatusDesc = value; OnPropertyChanged("ChannelStatusDesc"); } }
        }
        #endregion properties

        #region Events
        #endregion Events




        #region constructor
        /// <summary>
        /// 
        /// </summary>
        public PowerMeterModel()
        {

        }
        #endregion constructor






        #region PowerMeterSpecifiekeFunctions
        /// <summary>
        /// 
        /// </summary>
        public override void StartConnection()
        {
            base.StartConnection();
            ChannelStatusDesc = "VERANDER ME TERUG"; //GetChannelStatusDescription();
        }
        /// <summary>
        /// Checks if a sensor is connected
        /// </summary>
        /// <returns>Sensor Status</returns>
        public int GetChannelStatus()
        {
            return int.Parse(WriteCommandRes(Power.CHANNELSTATUS));
        }



        /// <summary> 
        /// Gets the ChannelStatus and checks the description
        /// </summary>
        /// <returns>Description for sensor status</returns>
        public string GetChannelStatusDescription()
        {
            int stat = GetChannelStatus();
            String result = null;
            switch (stat)
            {
                case 0:
                    result = COND_NO_SENSOR;
                    break;
                case +2:
                    result = COND_CONNECTED_A;
                    break;
                case +4:
                    result = COND_CONNECTED_B;
                    break;
                case +6:
                    result = COND_CONNECTED_A_B;
                    break;
                case +8:
                    result = COND_ERROR_A;
                    break;
                case +16:
                    result = COND_ERROR_B;
                    break;
                default:
                    break;

            }
            ChannelStatusDesc = result;
            return result;
        }



        /// <summary>
        /// Zero and callibration of the device 
        /// </summary>
        /// <returns>Succeeded when result is 0</returns>
        public string Calibrate()
        {
            string result = String.Empty;
            try
            {
                result = WriteCommandRes(Power.CALIBRATE);
            }
            catch (Exception e)
            {
                result = "Calibration error: " + e.ToString();
                throw;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Args">If true, powermeter will use FetchArgs functions instead</param>
        /// <returns></returns>
        public string Fetch(bool args=false)
        {
            string result = "";
            try
            {
                result = (args ==false)?WriteCommandRes(Power.FETCH):WriteCommandRes(Power.FETCHARG);
            }
            catch (Exception e)
            {
                result = "Couldn't fetch data: " + e.ToString();
                throw;
            }
            return result;
        }

        #endregion PowerMeterSpecifiekeFunctions
    }
}
