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
using Automatisation.Model.Base;

namespace Automatisation.Logging.Targets
{
    [Target("TargetAllLogs")]
    public class TargetAllLogs : TargetWithLayout
    {
        private readonly Subject<string> _messages = new Subject<string>();
        public TargetAllLogs()
        {
            Messages = _messages;

        }
        
        protected override void Write(LogEventInfo logEvent)
        {
            string item = Layout.Render(logEvent);
            _messages.OnNext(item);
        }
        
        public IObservable<string> Messages { get; private set; }
    }
}
