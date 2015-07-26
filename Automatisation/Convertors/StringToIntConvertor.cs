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
using System.Globalization;
using System.Windows.Data;

namespace Automatisation.Convertors
{
    class StringToIntConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(int))
                throw new InvalidOperationException("Target must be an int");

            int i = 0;
            string s = value as string;
            if (s != null)
                int.TryParse(s, out i);

            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if( targetType != typeof(string))
                throw new InvalidOperationException("Target must be a string");

            return value.ToString();
        }
    }
}
