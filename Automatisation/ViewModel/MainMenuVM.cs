/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace Automatisation.ViewModel
{
    class MainMenuVM : ObservableObject
    {
        #region fields
        //File menu
        private RelayCommand _menuFileNew;
        private RelayCommand _menuFileSave;
        private RelayCommand _menuFileExit;
        //Settings Menu
        private RelayCommand _menuConfigIP;
        private RelayCommand _menuConfigTek;
        //Help Menu
        private RelayCommand _menuHelpAgilentUser;
        private RelayCommand _menuHelpAgilentProg;
        private RelayCommand _menuHelpTekUser;
        private RelayCommand _menuHelpTekProg;
        private RelayCommand _menuHelpAttManual;
        private RelayCommand _menuHelpAttAPI;
        private RelayCommand _menuHelpIperf;
        private RelayCommand _menuHelpAirmagnet;
        private RelayCommand _menuHelpBachelorproef;
        #endregion fields

        #region properties
        public RelayCommand MenuFileNew { get { return _menuFileNew ?? (_menuFileNew = new RelayCommand(TempFunction)); } }
        public RelayCommand MenuFileSave { get { return _menuFileSave ?? (_menuFileSave = new RelayCommand(TempFunction)); } }
        public RelayCommand MenuFileExit { get { return _menuFileExit ?? (_menuFileExit = new RelayCommand(TempFunction)); } }

        //Settings Menu
        public RelayCommand MenuConfigIP { get { return _menuConfigIP ?? (_menuConfigIP = new RelayCommand(SettingsIPShow)); } }
        public RelayCommand MenuConfigTek { get { return _menuConfigTek ?? (_menuConfigTek = new RelayCommand(SettingsSpectrumShow)); } }

        //Help Menu
        public RelayCommand MenuHelpAgilentUser { get { return _menuHelpAgilentUser ?? (_menuHelpAgilentUser = new RelayCommand(Resources.PDF.OpenAgilentUserGuide)); } }
        public RelayCommand MenuHelpAgilentProg { get { return _menuHelpAgilentProg ?? (_menuHelpAgilentProg = new RelayCommand(Resources.PDF.OpenAgilentProgrammingGuide)); } }
        public RelayCommand MenuHelpTekUser { get { return _menuHelpTekUser ?? (_menuHelpTekUser = new RelayCommand(Resources.PDF.OpenTekUserGuide)); } }
        public RelayCommand MenuHelpTekProg { get { return _menuHelpTekProg ?? (_menuHelpTekProg = new RelayCommand(Resources.PDF.OpenTekProgrammingGuide)); } }
        public RelayCommand MenuHelpAttManual { get { return _menuHelpAttManual ?? (_menuHelpAttManual = new RelayCommand(Resources.PDF.OpenAttManual)); } }
        public RelayCommand MenuHelpAttAPI { get { return _menuHelpAttAPI ?? (_menuHelpAttAPI = new RelayCommand(Resources.PDF.OpenAttAPIGuide)); } }
        public RelayCommand MenuHelpIperf { get { return _menuHelpIperf ?? (_menuHelpIperf = new RelayCommand(Resources.PDF.OpenIperfGuide)); } }
        public RelayCommand MenuHelpAirmagnet { get { return _menuHelpAirmagnet ?? (_menuHelpAirmagnet = new RelayCommand(Resources.PDF.OpenAirmagnetGuide)); } }
        public RelayCommand MenuHelpBachelor { get { return _menuHelpBachelorproef ?? (_menuHelpBachelorproef = new RelayCommand(Resources.PDF.OpenBachelor)); } }
        #endregion properties
        //VERSCHIL MET ????
        /*
         *  public ICommand LoginCommand
        {
            get { return new RelayCommand(Login, canExecuteLogin); }
        }
         * */


        #region constructor
        public MainMenuVM()
        {
        }
        #endregion constructor

        /// <summary>
        ///Send notification to MainView to launch a new Settings IP Window
        /// </summary>
        private void SettingsIPShow()
        {
            ApplicationVM.SettingsIPShow();
        }

        /// <summary>
        ///Send notification to MainView to launch a new Settings Spectrum Window
        /// </summary>
        private void SettingsSpectrumShow()
        {
            ApplicationVM.SettingsSpectrumShow();
        }

        /// <summary>
        /// Used for not implemented menuitems
        /// </summary>
        private void TempFunction()
        {
            Messenger.Default.Send(new NotificationMessage(Messages.Messages.NOT_IMPLEMENTED));
        }
    }


}
