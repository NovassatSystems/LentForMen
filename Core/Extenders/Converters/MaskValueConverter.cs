using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Core
{
    public class MaskValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var text = value.ToString();
            var masks = parameter.ToString().Split('|').OrderBy(o => o.Length).ToDictionary(s => Unmask(s).Length, s => s);

            var str = UnmaskText(text);
            text = string.Empty;
            var mask = masks.FirstOrDefault(w => str.Length <= w.Key).Value ?? masks.Last().Value;

            var count = 0;
            foreach (var item in mask)
            {
                if (count >= str.Length)
                    break;

                if (item != '#')
                    text += item;
                else
                {
                    text += str[count];
                    count++;
                }
            }

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null ? null : Unmask(value.ToString() ?? string.Empty).Replace(" ", "");

        string UnmaskText(string value)
            => Unmask(value).Replace(" ", "");

        string Unmask(string value)
            => value.Replace(".", "").Replace("-", "").Replace("/", "").Replace("(", "").Replace(")", "");
    }
}
