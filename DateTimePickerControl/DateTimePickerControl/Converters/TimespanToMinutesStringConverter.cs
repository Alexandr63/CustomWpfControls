using System;
using System.Globalization;
using System.Windows.Data;

namespace DateTimePickerControl.Converters
{
    public class TimespanToMinutesStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = ((TimeSpan)value).Minutes.ToString();
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
