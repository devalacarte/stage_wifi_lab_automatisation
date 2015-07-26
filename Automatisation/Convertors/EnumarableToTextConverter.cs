using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Automatisation.Convertors
{
    public class EnumarableToTextConverter : IValueConverter
    {
        public object Convert(
         object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            if (value is IEnumerable)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in value as IEnumerable)
                {
                    sb.AppendLine(s.ToString());
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(
         object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
