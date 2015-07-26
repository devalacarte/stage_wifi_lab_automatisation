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
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Automatisation.Logging.Targets
{
    [Target("MeasTargetLogs")]
    public class MeasTargetLogs : TargetWithLayout
    {
        private readonly Subject<string> _messages = new Subject<string>();

        public MeasTargetLogs()
        {
            Messages = _messages;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent.FormattedMessage.ToString().ToUpper().Contains(TargetNames.MEAS))
            {
                string item = Layout.Render(logEvent);
                _messages.OnNext(item);
            }
        }

        public IObservable<string> Messages { get; private set; }
    }
}
