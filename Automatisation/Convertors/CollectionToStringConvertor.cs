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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
namespace Automatisation.Convertors
{
    class CollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("Target must be a string");
            return String.Join("\r\n", ((ObservableCollection<string>)value).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return null;
            if (targetType != typeof(ObservableCollection<string>))
                throw new InvalidOperationException("Target must be an ObservableCollection<string>");
            ObservableCollection<string> list = new ObservableCollection<string>();
            foreach (string s in value.ToString().Split('\r'))
            {
                string val = s.Replace('\n', ' ');
                val = val.Trim();
                if (!string.IsNullOrEmpty(val))
                    list.Add(val);
            }

            return list;
        }
    }

    public class CollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<string> messages = (ObservableCollection<string>)value;

            var sb = new StringBuilder();
            foreach (string msg in messages)
            {
                sb.AppendLine(msg);
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ObservableStringCollectionToMultiLineStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<string> logEntries = values[0] as ObservableCollection<string>;
            StringBuilder sb = new StringBuilder();
            if (logEntries != null && logEntries.Count > 0)
            {
                /*
                 A first chance exception of type 'System.InvalidOperationException' occurred in mscorlib.dll
                 An unhandled exception of type 'System.InvalidOperationException' occurred in mscorlib.dll
                 Additional information: Collection was modified; enumeration operation may not execute.
                 * */
                try
                {
                    foreach (string msg in logEntries)
                    {
                        sb.AppendLine(msg);
                    }
                    return sb.ToString();
                }
                catch (Exception)
                {
                    //throw;
                }

                return "Log convert error";
                
            }
            else
                return String.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
