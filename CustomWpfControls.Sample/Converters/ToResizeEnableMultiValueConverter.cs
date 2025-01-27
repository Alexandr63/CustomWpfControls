using System;
using System.Globalization;
using System.Windows.Data;
using CustomWpfControls.Sample.ViewModels;

namespace CustomWpfControls.Sample.Converters
{
    public class ToResizeEnableMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool resizeEnable = (bool) values[0];
            ListFillType listFillType = (ListFillType) values[1];

            return resizeEnable && listFillType != ListFillType.AutoSizeColumn && listFillType != ListFillType.AutoSizeRow;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
