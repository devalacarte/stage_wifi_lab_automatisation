/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */

namespace Automatisation.Logging
{
    
    public static class TargetNames
    {
        public static readonly string ALL = "TARGETALLLOGS: ";
        public static readonly string POWER = "TARGETPOWERLOGS: ";
        public static readonly string SPECTRUM = "TARGETSPECTRUMLOGS: ";
        public static readonly string IPERF = "TARGETIPERF:";
        public static readonly string ATT = "TARGETATTENUATORLOGS: ";
        public static readonly string MEAS = "TARGETALLMEASUREMENTSLOG: ";
        public static readonly string FILE = "TARGETFORFILELOGGING: ";
    }
    /// <summary>
    /// Code behind config for custom targets with NLog instead of using Nlog.config file
    /// </summary>
    public static class Config
    {
        private static NLog.Config.LoggingConfiguration config;
        private static Logging.Targets.TargetAllLogs _targetAll;
        private static Logging.Targets.TargetIperfLogs _targetIClient;
        /// <summary>
        /// Setup custom targets and rules
        /// </summary>
        public static void SetupConfig()
        {
           //get current config
            config = NLog.LogManager.Configuration;
            NLog.LogManager.ThrowExceptions = true;
            SetupTargets();

            SetupRules();

            //save config
            NLog.LogManager.Configuration = config;
        }

        private static void SetupTargets()
        {
            //Create a new target
            //Give target a name
            //add target to config
            //config = new NLog.Config.LoggingConfiguration();
            _targetAll = new Logging.Targets.TargetAllLogs();
            _targetAll.Name = TargetNames.ALL;
            _targetAll.Layout = "${message}";
            config.AddTarget(TargetNames.ALL, _targetAll);

            _targetIClient = new Logging.Targets.TargetIperfLogs();
            _targetIClient.Name = TargetNames.IPERF;
            config.AddTarget(TargetNames.IPERF, _targetIClient);

            /*NLog.Targets.FileTarget target = new NLog.Targets.FileTarget();
            target.CreateDirs = true;
            target.KeepFileOpen = false;
            target.Layout = "${message}";
            target.FileName = "${basedir}/log.txt";
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Info, target));*/
        }

        private static void SetupRules()
        {
            //add rule to target
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Info, _targetAll));
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Info, _targetIClient));
        }

        /// <summary>
        /// Get an existing target by name
        /// </summary>
        /// <param name="target">Target Name</param>
        /// <returns>Nlog Target</returns>
        public static NLog.Targets.Target GetTarget(string target)
        {
            foreach (var item in NLog.LogManager.Configuration.AllTargets)
            {
                if (item.Name == target)
                {
                    //var target = item as TargetAllLogs;
                    //target.Messages.Subscribe(msg => _messages.Add(msg));
                    return item;
                }
            } return null;
        }

    }
}
