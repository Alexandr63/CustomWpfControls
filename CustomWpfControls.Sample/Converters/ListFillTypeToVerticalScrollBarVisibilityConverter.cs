using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using CustomWpfControls.Sample.ViewModels;

namespace CustomWpfControls.Sample.Converters
{
    public class ListFillTypeToVerticalScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ListFillType)value)
            {
                case ListFillType.AutoSizeColumn:
                case ListFillType.Column:
                case ListFillType.Table:
                case ListFillType.Wrap:
                case ListFillType.Row:
                    return ScrollBarVisibility.Auto;
                case ListFillType.AutoSizeRow:
                    return ScrollBarVisibility.Disabled;
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
