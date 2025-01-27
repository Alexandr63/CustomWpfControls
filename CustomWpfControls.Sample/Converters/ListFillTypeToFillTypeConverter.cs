using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using CustomWpfControls.Sample.ViewModels;

namespace CustomWpfControls.Sample.Converters
{
    public class ListFillTypeToFillTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ListFillType)value)
            {
                case ListFillType.AutoSizeColumn:
                case ListFillType.Column:
                    return FillType.Column;
                case ListFillType.AutoSizeRow:
                case ListFillType.Row:
                    return FillType.Row;
                case ListFillType.Table:
                    return FillType.Table;
                case ListFillType.Wrap:
                    return FillType.Wrap;
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
