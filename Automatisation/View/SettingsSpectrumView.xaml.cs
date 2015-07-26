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
using System.Windows.Shapes;

namespace Automatisation.View
{
    /// <summary>
    /// Interaction logic for SettingsSpectrumView.xaml
    /// </summary>
    public partial class SettingsSpectrumView : Window
    {
        public SettingsSpectrumView()
        {
            InitializeComponent();
        }

        private void FilterShape_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox fs = (ComboBox)sender;
            if (fs.SelectedItem.ToString().Contains("NYQ") || fs.SelectedItem.ToString().Contains("RNYQ"))
                RollOffRatio.IsEnabled = true;
            else
                RollOffRatio.IsEnabled = false;
        }

        private void cboMeasure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox fs = (ComboBox)sender;
            if (fs.SelectedItem.ToString().Contains("CHP"))
                grdCHP.IsEnabled = true;
            else
                grdCHP.IsEnabled = false;
        }
    }
}
