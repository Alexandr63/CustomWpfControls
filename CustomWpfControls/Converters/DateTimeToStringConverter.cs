using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomWpfControls.Converters
{
    public class DateTimeToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)values[0];
            string formatString = (string)values[1];

            if (string.IsNullOrEmpty(formatString))
            {
                return dateTime.ToString("g", CultureInfo.CurrentUICulture);
            }

            return dateTime.ToString(formatString);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
