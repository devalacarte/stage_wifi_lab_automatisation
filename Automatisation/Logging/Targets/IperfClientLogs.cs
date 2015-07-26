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
    [Target("TargetIperfLogs")]
    public class TargetIperfLogs : TargetWithLayout
    {
        private readonly Subject<string> _messages = new Subject<string>();

        public TargetIperfLogs()
        {
            Messages = _messages;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent.FormattedMessage.ToString().ToUpper().Contains(TargetNames.IPERF))
            {
                string item = Layout.Render(logEvent);
                _messages.OnNext(item);
            }
        }

        public IObservable<string> Messages { get; private set; }
    }
}
