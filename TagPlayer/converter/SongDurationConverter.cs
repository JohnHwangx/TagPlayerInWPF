using System;
using System.Globalization;
using System.Windows.Data;

namespace TagPlayer.converter
{
	public class SongDurationConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var duration = value as string;
			var hour = duration?.Split(':')[0];
			if (hour != null && int.Parse(hour) == 0)
			{
				duration = duration.Substring(duration.IndexOf(":", StringComparison.Ordinal)+1);
			}
			return duration;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}