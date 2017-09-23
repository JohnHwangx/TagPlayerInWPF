using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProgressBar
{
    public class Converters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bl = (bool)value;
            return bl ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}