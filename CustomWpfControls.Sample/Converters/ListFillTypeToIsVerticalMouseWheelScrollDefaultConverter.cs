using CustomWpfControls.Sample.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomWpfControls.Sample.Converters
{
    public class ListFillTypeToIsVerticalMouseWheelScrollDefaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListFillType listFillType = (ListFillType) value;

            if (listFillType == ListFillType.AutoSizeRow || listFillType == ListFillType.Row)
            {
                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
