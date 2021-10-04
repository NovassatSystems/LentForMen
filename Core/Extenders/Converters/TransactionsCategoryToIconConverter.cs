using System;
using System.Globalization;
using Xamarin.Forms;

namespace Core
{
    public class TransactionsCategoryToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string category)
            {
                switch (category)
                {
                    case "gas":
                        return "ic_gas";
                    case "constructions":
                        return "ic_constructions";
                    case "restaurant":
                        return "ic_restaurant";
                    case "health":
                        return "ic_health";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
