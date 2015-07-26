/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Automatisation.Messages
{
    /// <summary>
    /// Messagebox.show with result for MVVM Light Messaging
    /// </summary>
    class ShowSaveMessage : NotificationMessageAction<MessageBoxResult>
    {
        /// <summary>
        /// Messagebox.show with result for MVVM Light Messaging
        /// </summary>
        public ShowSaveMessage(object Sender, Action<MessageBoxResult> callback)
            : base(Sender, "GetPassword", callback)
        {

        }
    }
}
