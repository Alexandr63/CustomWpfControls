using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomWpfControls.Converters
{
    public class TimespanToHoursStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = ((TimeSpan) value).Hours.ToString();
            if (str.Length == 1)
            {
                str = $"0{str}";
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
