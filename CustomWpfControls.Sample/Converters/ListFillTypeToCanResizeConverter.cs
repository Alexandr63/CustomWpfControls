using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using CustomWpfControls.Sample.ViewModels;

namespace CustomWpfControls.Sample.Converters
{
    public class ListFillTypeToCanResizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ListFillType)value)
            {
                case ListFillType.AutoSizeColumn:
                case ListFillType.AutoSizeRow:
                    return false;
                case ListFillType.Column:
                case ListFillType.Row:
                case ListFillType.Table:
                case ListFillType.Wrap:
                    return true;
                default:
                    throw new InvalidEnumArgumentException(value.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
