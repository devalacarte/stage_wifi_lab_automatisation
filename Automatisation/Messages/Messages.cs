/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatisation.Messages
{
    /// <summary>
    /// Messages to use as notification message for MVVM Light's framework
    /// </summary>
    public static class Messages
    {
        //ip settings
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: LaunchIpSettingsNewWindowAndLoadOldSettings
        /// </summary>
        public const string SETTINGSIP_LAUNCH = "LaunchIpSettingsNewWindowAndLoadOldSettings";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: SaveIpSettings
        /// </summary>
        public const string SETTINGSIP_SAVE = "SaveIpSettings";
        public const string SETTINGSIP_PING = "ShowPingResuly";

        //power settings
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: LaunchPowerSettingsNewWindowAndLoadCurrentSettings
        /// </summary>
        public const string SETTINGSPOWER_LAUNCH = "LaunchPowerSettingsNewWindowAndLoadCurrentSettings";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: SavePowerSettings
        /// </summary>
        public const string SETTINGSPOWER_SAVE = "SavePowerSettings";

        //spectrum settings
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views: LaunchSpectrumSettingsNewWindowAndLoadCurrentSettings
        /// </summary>
        public const string SETTINGSSPECTRUM_LAUNCH = "LaunchSpectrumSettingsNewWindowAndLoadCurrentSettings";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: CloseSpectrumSettingsNoSavingNeeded
        /// </summary>
        public const string SETTINGSSPECTRUM_CLOSE = "CloseSpectrumSettingsNoSavingNeeded";

        //iperf settings

        //attenuator settings

        //powertab
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: LaunchPowerMeterAfterEnablingViewFromMenu
        /// </summary>
        public const string POWER_ENABLE = "LaunchPowerMeterAfterEnablingViewFromMenu";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: DisablePowerMeterAfterDisablingViewFromMenu
        /// </summary>
        public const string POWER_DISABLE = "DisablePowerMeterAfterDisablingViewFromMenu";

        //spectrum tab
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: LaunchSpectrumMeterAfterEnablingViewFromMenu
        /// </summary>
        public const string SPECTRUM_ENABLE = "LaunchSpectrumMeterAfterEnablingViewFromMenu";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: DisableSpectrumMeterAfterDisablingViewFromMenu
        /// </summary>
        public const string SPECTRUM_DISABLE = "DisableSpectrumMeterAfterDisablingViewFromMenu";

        //IPERFTab
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: LaunchIPERFMeterAfterEnablingViewFromMenu
        /// </summary>
        public const string IPERF_ENABLE = "LaunchIPERFMeterAfterEnablingViewFromMenu";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: DisableIPERFMeterAfterDisablingViewFromMenu
        /// </summary>
        public const string IPERF_DISABLE = "DisableIPERFMeterAfterDisablingViewFromMenu";
        public const string IPERF_SERVER_CMD = "CommandForSSHServer";
        public const string IPERF_CLIENT_CMD = "CommandForSSHClient";

        //AttenuatorTab
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: LaunchAttenuatorsAfterEnablingViewFromMenu
        /// </summary>
        public const string ATTENUATOR_ENABLE = "LaunchAttenuatorsAfterEnablingViewFromMenu";
        /// <summary>
        /// Message used for MVVM Lights Messaging System to send data from viewmodels to views:: DisableAttenuatorsAfterDisablingViewFromMenu
        /// </summary>
        public const string ATTENUATOR_DISABLE = "DisableAttenuatorsAfterDisablingViewFromMenu";
        /// <summary>
        /// TestModeIsEnabledGoToTabAttenuatorViewModelAndSetIsTestModeToZero
        /// </summary>
        public const string ATTENUATOR_TEST_MODE = "TestModeIsEnabledGoToTabAttenuatorViewModelAndSetIsTestModeToZero";

        /// <summary>
        /// ShowMessageBoxCurrentValueIsLowerThanMinVal
        /// </summary>
        public const string ATT_LTMIN = "ShowMessageBoxCurrentValueIsLowerThanMinVal";
        /// <summary>
        /// ShowMessageBoxCurrentValueIsGreaterThanMaxVal
        /// </summary>
        public const string ATT_GTMAX = "ShowMessageBoxCurrentValueIsGreaterThanMaxVal";


        //general
        /// <summary>
        /// ShowNotImplementedMessageBox
        /// </summary>
        public const string NOT_IMPLEMENTED = "ShowNotImplementedMessageBox";
    }
}
