using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatisation.Logging.Targets
{
    public class OutputTarget : Target
    {
        public Action<string> Log = delegate { };

        public OutputTarget(string name, LogLevel level)
        {
            LogManager.Configuration.AddTarget(name, this);
            SimpleConfigurator.ConfigureForTargetLogging(this, level);
        }

        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                Write(logEvent);
            }
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            Write(logEvent.LogEvent);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            Log(logEvent.FormattedMessage);
        }
    }
}
