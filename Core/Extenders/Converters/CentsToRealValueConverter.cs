using System;
using System.Globalization;
using Xamarin.Forms;

namespace Core
{
    public class CentsToRealValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int cents)
                return cents / 100m;       

            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value;
    }
}
