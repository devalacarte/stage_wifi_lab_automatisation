/*
     .-'
'--./ /     _.---.
'-,  (__..-`       \
   \          .     |
    `,.__.   ,__.--/
      '._/_.'___.-`

     FREE WILLY 
 */
using System;
using System.Diagnostics;
using System.IO;
namespace Automatisation.Resources
{
    /// <summary>
    /// Names for pdfs
    /// </summary>
    public static class UserGuides
    {
        public static string AG_USER = "agilent_user_guide.pdf";
        public static string AG_PROG = "agilent_programming_guide.pdf";
        public static string TEK_USER = "tektronix_user_guide.pdf";
        public static string TEK_PROG = "tektronix_programming_guide.pdf";
        public static string ATT_USER = "attenuator_manual.pdf";
        public static string ATT_API = "attenuator_API_Instructions.pdf";
        public static string IPERF = "iperf.pdf";
        public static string AIR = "airmagnet_user_guide.pdf";
        public static string BACHELOR = "bachelorproef_xavier.pdf";
    }


    public static class PDF
    {
        /// <summary>
        /// Opens the user manual for Agilent N1991A
        /// </summary>
        public static void OpenAgilentUserGuide()
        {
            OpenPDF(UserGuides.AG_USER, Properties.Resources.Agilent_User_Guide);
        }
        
        /// <summary>
        /// Opens the programming manual for Agilent N1191A
        /// </summary>
        public static void OpenAgilentProgrammingGuide()
        {
            OpenPDF(UserGuides.AG_PROG, Properties.Resources.Agilent_Programming_Guide);
        }

        /// <summary>
        /// Opens the user manual for Tektronix' RSA3308A
        /// </summary>
        public static void OpenTekUserGuide()
        {
            OpenPDF(UserGuides.TEK_USER, Properties.Resources.Tektronix_User_Guide);
        }

        /// <summary>
        /// Opens the programming manual for Tektronix' RSA3308A
        /// </summary>
        public static void OpenTekProgrammingGuide()
        {
            OpenPDF(UserGuides.TEK_PROG, Properties.Resources.Tektronix_Programming_Guide);
        }

        /// <summary>
        /// Opens the user manual for the attenuators
        /// </summary>
        public static void OpenAttManual()
        {
            OpenPDF(UserGuides.ATT_USER, Properties.Resources.Attenuator_Manual);
        }

        /// <summary>
        /// Opens the programming API guide for the attenuators
        /// </summary>
        public static void OpenAttAPIGuide()
        {
            OpenPDF(UserGuides.ATT_API, Properties.Resources.Attenuator_API_Instructions);
        }

        /// <summary>
        /// Opens the user manual for IPERF
        /// </summary>
        public static void OpenIperfGuide()
        {
            OpenPDF(UserGuides.IPERF, Properties.Resources.Iperf);
        }

        /// <summary>
        /// Opens the user manual for Airmagnet's WifiAnalyzer
        /// </summary>
        public static void OpenAirmagnetGuide()
        {
            OpenPDF(UserGuides.AIR, Properties.Resources.Airmagnet_User_Guide);
        }

        /// <summary>
        /// Opens Bachelorproef
        /// </summary>
        public static void OpenBachelor()
        {
            OpenPDF(UserGuides.BACHELOR, Properties.Resources.bachelorproef_xavier);
        }
        
        
        /// <summary>
        /// General function to open a resource file
        /// </summary>
        /// <param name="filename">New filename</param>
        /// <param name="resourcefile">File from resources</param>
        private static void OpenPDF(string filename, Byte[] resourcefile)
        {
            Byte[] bytes = resourcefile;
            using (FileStream fs = File.Create(filename))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            Process.Start(filename);
        }
    }
}
