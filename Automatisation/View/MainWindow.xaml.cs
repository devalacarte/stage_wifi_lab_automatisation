using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace Automatisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));  
            InitializeComponent();
        }

        #region events
        /// <summary>
        /// A handler to do an action when a Message has been received
        /// </summary>
        /// <param name="m">A notification message send through the Messenger service from MVVM Light</param>
        private void NotificationMessageHandler(NotificationMessage m)
        {
            // Checks the actual content of the message
            switch (m.Notification)
            {
                case Messages.Messages.SETTINGSIP_LAUNCH:
                    Automatisation.View.SettingsIPView settings_ip = new View.SettingsIPView();
                    settings_ip.Show();
                    break;
                case Messages.Messages.SETTINGSSPECTRUM_LAUNCH:
                    Automatisation.View.SettingsSpectrumView settings_spectrum = new View.SettingsSpectrumView();
                    settings_spectrum.Show();
                    break;
                case Messages.Messages.ATTENUATOR_TEST_MODE:
                    MessageBox.Show("Attenuators test mode is enabled, disable testmode in tabAttenuatorViewModel");
                    break;
                case Messages.Messages.NOT_IMPLEMENTED:
                    MessageBox.Show("This function is not yet implemented");
                    break;
                default:
                    break;
            }
        }
        #endregion events
    }
}
    