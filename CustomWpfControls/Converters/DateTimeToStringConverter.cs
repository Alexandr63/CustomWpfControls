using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomWpfControls.Converters
{
    public class DateTimeToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dateTime = (DateTime?)values[0];
            string formatString = (string)values[1];
            
            return ConvertToString(dateTime, formatString);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string ConvertToString(DateTime? dateTime, string formatString)
        {
            if (!dateTime.HasValue)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(formatString))
            {
                return dateTime.Value.ToString("g", CultureInfo.CurrentUICulture);
            }

            return dateTime.Value.ToString(formatString);
        }
    }
}
