using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatisation.Messages
{
    public class SelectFolderMessage
    {
        public Action<string> CallBack { get; set; }
        public SelectFolderMessage(Action<string> callback)
        {
            CallBack = callback;
        }
    }
}
