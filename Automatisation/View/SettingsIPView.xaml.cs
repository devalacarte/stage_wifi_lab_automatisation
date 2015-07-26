using Automatisation.Messages;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;

namespace Automatisation.View
{
    /// <summary>
    /// Interaction logic for SettingsIPView.xaml
    /// </summary>
    public partial class SettingsIPView : Window
    {
        public SettingsIPView()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));
            Messenger.Default.Register<NotificationMessageAction<MessageBoxResult>>(this, (m) => NotificationMessageActionHandler(m));
            Messenger.Default.Register<NotificationMessage<string>>(this, (message) => NotificationMessageHandlerString(message));
            Messenger.Default.Register<Messages.SelectFolderMessage>(this, SelectFolderHandler);
            Unloaded += SettingsIPView_Unloaded;

            InitializeComponent();
        }

        private void NotificationMessageHandler(NotificationMessage m)
        {
            // Checks the actual content of the message.
            /*switch (m.Notification)
            {
                case "blablablabla":
                    MessageBox.Show("blablablablabl");
                    break;
                default:
                    break;
            }*/
        }

        private void NotificationMessageHandlerString(NotificationMessage<string> msg)
        {
            // Checks the actual content of the message.
            switch (msg.Notification)
            {
                case Messages.Messages.SETTINGSIP_PING:
                    MessageBox.Show(msg.Content);
                    break;
                default:
                    break;
            }
        }

        private void NotificationMessageActionHandler(NotificationMessageAction<MessageBoxResult> m)
        {
            if (m.Notification == Messages.Messages.SETTINGSIP_SAVE)
            {
                var result = MessageBox.Show("Do you want to save the settings?", "Save settings?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                m.Execute(result);
            }
        }

        private void SelectFolderHandler(SelectFolderMessage msg)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderdialg = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderdialg.ShowNewFolderButton = false;
                folderdialg.RootFolder = Environment.SpecialFolder.MyComputer;

                folderdialg.Description = "Choose a location for logfiles";
                folderdialg.ShowDialog();
                if (folderdialg.SelectedPath != null)
                {
                    msg.CallBack(folderdialg.SelectedPath);
                }
            }
        }

        void SettingsIPView_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
