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
    public static class SCPI
    {
        /// <summary>
        /// The *IDN? query allows the power meter to identify itself.
        /// Result: Vendor, Model, S/N, Firmware
        /// Agilent Programming Guide p773
        /// </summary>
        public static string IDN = "*IDN?";

        /// <summary>
        /// The *CLS (CLear Status) command clears the status data structures. 
        /// Agilent Programming Guide p739
        /// </summary>
        public static string CLS = "*CLS";

        /// <summary>
        /// The *RST (ReSeT) command places the power meter in a known state.
        /// Agilent Programming Guide p749
        /// </summary>
        public static string RST = "*RST"; //Resets the instrument specific functionality.

        /// <summary>
        /// The *ESE (Event Status Enable) <NRf> command sets the Standard Event Status Enable Register. 
        /// This register contains a mask value for the bits to be enabled in the Standard Event Status Register.
        /// Agilent Programming Guide p742
        /// </summary>
        public static string ESE = "*ESE 1"; //OPC bit van ESER

        /// <summary>
        /// The *SRE <NRf> command sets the Service Request Enable register bits.
        /// This register contains a mask value for the bits to be enabled in the Status Byte Register.
        /// Agilent Programming Guide p74
        /// </summary>
        public static string SRE = "*SRE 32"; //ESB bit van SRER

        /// <summary>
        /// The *OPC (OPeration Complete) command causes the power meter to set the operation complete bit in the Standard Event Status Register when all pending device operations have completed.
        /// Set the Operation Complete bit in the Event Status Register to '1' when all pending commands and/or queries are finished.
        /// Agilent Programming Guide p774
        /// </summary>
        public static string OPC = "*OPC";

        /// <summary>
        /// OPeration Complete command.
        /// The controller can read this bit with the *ESR? query. 
        /// The controller can also manipulate the status system in a way that, for instance, the instrument will notify the controller with a SRQ.
        /// Result = 1 -> All commands completed.
        /// </summary>
        public static string OPC_Q = "*OPC?";

        /// <summary>
        /// The DCL (Device Clear) command causes all GPIB instruments to assume a cleared condition. 
        /// The definition of device clear is unique for each instrument. For the power meter:
        /// • All pending operations are halted, that is, *OPC? and *WAI.
        /// • The parser (the software that interprets the programming codes) is reset and now expects to receive the first character of a programming code.
        /// • The output buffer is cleared.
        /// Will clear (initialize) all devices on the bus that have a device clear function, whether or not the controller has addressed them
        /// Agilent Programming Guide p786
        /// </summary>
        public static string DLC = "DCL";

        /// <summary>
        /// DCL but Clears or initializes all listen-addressed devices
        /// </summary>
        public static string SDC = "SDC";
    }
}