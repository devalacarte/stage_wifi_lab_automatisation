/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
namespace Automatisation.Commands
{
    public static class Power
    {
        /// <summary>
        /// This command causes the power meter to perform a calibration sequence on Channel A.
        /// Error -231: If the calibration or zeroing was not carried out successfully.
        /// Error -241: If there is no sensor connected.
        /// Agilent Programming Guide p231
        /// </summary>
        public static string CALIBRATE = "CAL1:ALL?";

        /// <summary>
        /// Query indicate whether a power sensor has been connected or disconnected.
        /// Agilent Programming Guide p501.
        /// </summary>
        public static string CHANNELSTATUS = "STATus:DEVice:CONDition?";

        /// <summary>
        /// Retrieves the upper window’s measurement.
        /// </summary>
        public static string FETCH = "FETC1?";

        /// <summary>
        /// Retrieves the upper window’s measurement.
        /// </summary>
        public static string FETCHARG = "FETC1? DEF,DEF,(@1)";
    }
}
