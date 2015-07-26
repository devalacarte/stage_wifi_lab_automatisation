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
    /// Interaction logic for SSHUC.xaml
    /// </summary>
    public partial class SSHUC : UserControl
    {
        public SSHUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WalburySoftware.TerminalControl terminal = this.WinFormHost.Child as WalburySoftware.TerminalControl;
            terminal.Connect();
            terminal.SetPaneColors(System.Drawing.Color.White, System.Drawing.Color.Black);
        }
    }
}
