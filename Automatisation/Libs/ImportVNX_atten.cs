/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using System.Runtime.InteropServices;

namespace Automatisation.Libs
{
    public enum VNX_Status : uint
    {
        STATUS_OK = 0,
        /// <summary>
        /// out of range input -- attenuation outside min/max etc.
        /// </summary>
        BAD_PARAMETER = 0x80010000,
        /// <summary>
        /// device isn't open, no handle, etc
        /// </summary>
        DEVICE_NOT_READY = 0x80030000,
        /// <summary>
        /// MSB is set if the device ID is invalid
        /// </summary>
        INVALID_DEVID = 0x80000000,
        /// <summary>
        /// LSB is set if a device is connected
        /// </summary>
        DEV_CONNECTED = 0x00000001,
        /// <summary>
        /// set if the device is opened
        /// </summary>
        DEV_OPENED = 0x00000002,
         /// <summary>
        /// set if the device is ramping (sweeping) the attenuation level
        /// </summary>
        SWP_ACTIVE = 0x00000004,
        /// <summary>
        /// set if the device is ramping up in attenuation level
        /// </summary>
        SWP_UP = 0x00000008,
        /// <summary>
        /// set if the device is in continuous sweep mode
        /// </summary>
        SWP_REPEAT = 0x00000010,
        // Internal values in DevStatus
        /// <summary>
        /// set if we don't want read thread updates of the device parameters
        /// </summary>
        DEV_LOCKED = 0x00000020,
        /// <summary>
        /// set when the read thread is running
        /// </summary>
        DEV_RDTHREAD = 0x00000040
    }

    public static class VNX_Atten
    {
        /// <summary>
        /// Name of the dll file used for the attenuators.
        /// </summary>
        private const string DLLLOCATION = "Libs\\VNX_atten.dll";

        /// <summary>
        /// Set testmode to FALSE for normal operation. 
        /// If testmode is TRUE the dll does not communicate with the actual hardware, but simulates the basic operation of the dll functions.
        /// In test mode there will be 2 devices, an LDA-102 and an LDA-602.
        /// </summary>
        /// <param name="arg1">0 false, 1 true</param>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetTestMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTestMode(int arg1);

        /// <summary>
        /// Get the number of connected attenuators attached to the system.
        /// </summary>
        /// <returns>Number of connected attenuators.</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetNumDevices", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNumDevices();

        /// <summary>
        /// Call fnLDA_GetDevInfo(DEVID *ActiveDevices), which will fill in the array with the device ids for each connected attenuator
        /// </summary>
        /// <param name="ActiveDevices">Allocate an array big enough to hold the device ids for the number of devices present</param>
        /// <returns>number of devices present on the machine</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetDevInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDevInfo([Out] uint[] ActiveDevices);

        /// <summary>
        /// call fnLDA_GetModelName(DEVID deviceID, char *ModelName) with a null ModelName pointer to get the length of the model name, or just use a buffer that can hold MAX_MODELNAME chars.
        /// Use the model name to identify the type of attenuator
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetModelName", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetModelName(uint deviceID, [Out] char[] ModelName);

        /// <summary>
        /// open the device interface to the attenuator and initialize the dll’s copy of the device’s settings. 
        /// If the fnLDA_InitDevice function succeeds, then you can use the various fnLDA_Get* functions to read the attenuator’s settings. 
        /// This function will fail, and return an error code if the attenuator has already been opened by another program.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>Error code if the function failed.</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_InitDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitDevice(uint deviceID);

        /// <summary>
        /// This function closes the device interface to the attenuator. 
        /// It should be called when your program is done using the attenuator.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_CloseDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CloseDevice(uint deviceID);

        /// <summary>
        /// Get the serial number of the attenuator.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetSerialNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSerialNumber(uint deviceID);

        /// <summary>
        /// obtain information about the status of a device, even before the device is initialized.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>status (described in attenuatiors.status class</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetDeviceStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceStatus(uint deviceID);

        /// <summary>
        /// set the attenuation level of the programmable attenuator. 
        /// The attenuation setting is encoded as an integer where each increment represents .25db of attenuation. 
        /// The encoding is: attenuation * .25db = Attenuation in db
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="attenuation">attenuation in db * 4</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetAttenuation", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAttenuation(uint deviceID, int attenuation);

        /// <summary>
        /// sets the attenuation level at the beginning of an attenuation ramp or sweep. 
        /// The encoding of rampstart, the attenuation level, is the same as the fnLDA_SetAttenuation function
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="rampstart">rampstart in db * 4</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetRampStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetRampStart(uint deviceID, int rampstart);

        /// <summary>
        /// sets the attenuation level at the end of an attenuation ramp or sweep. 
        /// The encoding of rampstop, the attenuation level, is the same as the fnLDA_SetAttenuation function.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="rampstop">rampstart in db * 4</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetRampEnd", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetRampEnd(uint deviceID, int rampstop);

        /// <summary>
        /// sets the size of the attenuation step that will be used to generate the attenuation ramp or sweep. 
        /// The encoding of attenuationstep, is the same as the fnLDA_SetAttenuation function. 
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="attenuation">stepsize in db * 4. Smallest step size is 2 (0.5db)</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetAttenuationStep", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAttenuationStep(uint deviceID, int attenuationStep);

        /// <summary>
        /// sets the length of time that the attenuator will dwell on each attenuation step while it is generating the attenuation ramp. 
        /// The dwelltime variable is encoded as the number of milliseconds to dwell at each level. 
        /// The minimum dwell time is 1 millisecond.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="attenuation">dwelltime in miliseconds (min 1ms)</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetDwellTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetDwellTime(uint deviceID, int dwelltime);

        /// <summary>
        /// sets the length of time that the attenuator will wait at the end of an attenuation ramp before beginning the ramp again when the ramp mode is set to SWP_REPEAT. 
        /// The idletime variable is encoded as the number of milliseconds to dwell at each level. 
        /// The minimum idle time is 0 milliseconds.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="attenuation">idletime in miliseconds (min 0ms)</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetIdleTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetIdleTime(uint deviceID, int idletime);

        /// <summary>
        /// Aallows rapid switching of the attenuator from its set value “on” (on = TRUE) to its maximum attenuation (on = FALSE).
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="on"></param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetRFOn", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetRFOn(uint deviceID, bool on);

        /// <summary>
        /// set the direction of the attenuation ramp. 
        /// To create a ramp with increasing attenuation, set up = TRUE. 
        /// Note that the ramp start attenuation value must be less than the ramp end attenuation value for a ramp with increasing attenuation. 
        /// For a ramp with decreasing attenuation the ramp start value must be greater than the ramp end value.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="up">up true, increase attenuation. End bigger than start</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetRampDirection", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetRampDirection(uint deviceID, bool up);

        /// <summary>
        /// select either a single ramp or sweep of attenuation values, or a repeating series of ramps. 
        /// If mode = TRUE then the ramp will be repeated, if mode = FALSE the ramp will only happen once.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="mode">mode true = repeat</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SetRampMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetRampMode(uint deviceID, bool mode);

        /// <summary>
        /// start and stop the attenuation ramps. 
        /// If go = TRUE the attenuator will begin sweeping, FALSE stops the sweep.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="go">go true = begin sweep</param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_StartRamp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartRamp(uint deviceID, bool go);

        /// <summary>
        /// The LabBrick attenuators can save their settings, and then resume operating with the saved settings when they are powered up. Set the desired parameters, 
        /// then use this function to save the settings.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_SaveSettings", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SaveSettings(uint deviceID);

        /// <summary>
        /// returns the current attenuation setting of the selected device. 
        /// When an attenuation ramp is active this value will change dynamically to reflect the current setting of the device. 
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>current attenuation in 0.25 db</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetAttenuation", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAttenuation(uint deviceID);

        /// <summary>
        /// returns the current attenuation ramp start value setting of the selected device.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>attenuation ramp start in 0.25db</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetRampStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRampStart(uint deviceID);

        /// <summary>
        /// returns the current attenuation ramp end setting of the selected device
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>attenuation ramp end in 0.25db</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetRampEnd", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRampEnd(uint deviceID);

        /// <summary>
        /// Returns the current dwell time for each step on the attenuation ramp in milliseconds.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>dwell time in ms</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetDwellTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDwellTime(uint deviceID);

        /// <summary>
        /// Returns the current idle time in ms, which is the delay between attenuation ramps when the device is in the repeating ramp mode.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>idle time in ms</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetIdleTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetIdleTime(uint deviceID);

        /// <summary>
        /// Returns the current attenuation step size setting of the selected device.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>attenuation step size in 0.25db</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetAttenuationStep", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAttenuationStep(uint deviceID);

        /// <summary>
        /// returns an integer value which is 1 when the attenuator is “on”, or 0 when the attenuator has been set “off” by the fnLDA_SetRFOn function. 
        /// Note that the function does not attempt to interpret attenuation settings as either “on” or “off”, 
        /// so if you set the attenuation level to 63 db, (attenuation = 252) the output signal level would be the same as if you had used the fnLDA_SetRFOn function with the on = FALSE, but this function would not return 0.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>1 = on</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetRF_On", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRFOn(uint deviceID);

        /// <summary>
        /// Returns the maximum attenuation value that the device can provide. 
        /// For the LDA-102 and LDA-602 programmable attenuators this value is 63 db, which is 252 .25 db units. 
        /// Since future products may have different maximum attenuation capabilities your software should use this function to obtain the maximum attenuation possible.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>maximum attenuation value that the device can provide</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetMaxAttenuation", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMaxAttenuation(uint deviceID);

        /// <summary>
        /// returns the minimum attenuation value that the device can provide. 
        /// For the LDA-102 and LDA-602 programmable attenuators this value is 0 db. 
        /// Since future products may have different capabilities your software should use this function to obtain the minimum attenuation possible.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns>minimum attenuation value that the device can provide</returns>
        [DllImport(DLLLOCATION, EntryPoint = @"fnLDA_GetMinAttenuation", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMinAttenuation(uint deviceID);
    }
}
