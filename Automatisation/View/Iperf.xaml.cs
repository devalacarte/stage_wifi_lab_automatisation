using Automatisation.Networking;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Automatisation.View
{
    /// <summary>
    /// Interaction logic for Iperf.xaml
    /// </summary>
    public partial class Iperf : UserControl
    {
        public Iperf()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage<string[]>>(this, (message) => NotificationMessageHandler(message));
        }
        private void NotificationMessageHandler(NotificationMessage<string[]> msg)
        {
            //something
            switch (msg.Notification)
            {
                case Messages.Messages.IPERF_CLIENT_CMD:
                    foreach (string s in msg.Content)
                    {
                        t1.SendText(s + '\r');
                    }
                    break;
                case Messages.Messages.IPERF_SERVER_CMD:
                    foreach (string s in msg.Content)
                    {
                        t2.SendText(s + '\r');
                    }
                    break;
                default:
                    break;
            }
        }

        private bool _firstLaunch = true;
        WalburySoftware.TerminalControl t1;
        WalburySoftware.TerminalControl t2;
        private void LoadedControl()
        {
            //WalburySoftware.TerminalControl t1 = new WalburySoftware.TerminalControl();
            //WalburySoftware.TerminalControl t2 = new WalburySoftware.TerminalControl("root", "luna1991", "192.168.17.129", WalburySoftware.ConnectionMethod.SSH2);
            
            t1 = this.WinFormCodeBehindBreakMVVMQuickAndDirty1.Child as WalburySoftware.TerminalControl;
            t1.Name = "IPERF Client";
            t1.Host = Properties.Settings.Default.IP_IPERF_CLIENT; t1.Password = Properties.Settings.Default.IPERFCLIENTPASS; t1.UserName = Properties.Settings.Default.IPERFCLIENTUSER; t1.Method = WalburySoftware.ConnectionMethod.SSH2;
            t1.Connect();

            t2 = this.WinFormCodeBehindBreakMVVMQuickAndDirty2.Child as WalburySoftware.TerminalControl;
            t2.Name = "IPERF Server";
            t2.Host = Properties.Settings.Default.IP_IPERF_SERVER; t2.Password = Properties.Settings.Default.IPERFSERVERPASS; t2.UserName = Properties.Settings.Default.IPERFSERVERUSER; t2.Method = WalburySoftware.ConnectionMethod.SSH2;
            t2.Connect();
            //crossthread error, code in control zelf aangepast, ieder lijntje aangepast heeft als commentaar //edited by xavier
            /*System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    t1 = this.WinFormCodeBehindBreakMVVMQuickAndDirty1.Child as WalburySoftware.TerminalControl;
                    t1.Host = Properties.Settings.Default.IP_IPERF_CLIENT; t1.Password = Properties.Settings.Default.IPERFCLIENTPASS; t1.UserName = Properties.Settings.Default.IPERFCLIENTUSER; t1.Method = WalburySoftware.ConnectionMethod.SSH2;
                    t1.Connect();
                }
            );*/
            /*System.Threading.ThreadPool.QueueUserWorkItem(
                o =>
                {
                    t2 = this.WinFormCodeBehindBreakMVVMQuickAndDirty2.Child as WalburySoftware.TerminalControl;
                    t2.Host = Properties.Settings.Default.IP_IPERF_SERVER; t2.Password = Properties.Settings.Default.IPERFSERVERPASS; t2.UserName = Properties.Settings.Default.IPERFSERVERUSER; t2.Method = WalburySoftware.ConnectionMethod.SSH2;
                    t2.Connect();
                }
            );*/
            //t1.SetPaneColors(System.Drawing.Color.White, System.Drawing.Color.Black);
            //t2.SetPaneColors(System.Drawing.Color.White, System.Drawing.Color.Black);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_firstLaunch)
                LoadedControl();
            _firstLaunch = false;
        }
    }
}
