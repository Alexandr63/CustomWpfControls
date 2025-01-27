using CustomWpfControls.Sample.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomWpfControls.Sample.Converters
{
    public class ToDoubleMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = (double)values[0];
            ListFillType listFillType = (ListFillType)values[1];

            if (listFillType == ListFillType.AutoSizeColumn || listFillType == ListFillType.AutoSizeRow)
            {
                return double.NaN;
            }

            return doubleValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
