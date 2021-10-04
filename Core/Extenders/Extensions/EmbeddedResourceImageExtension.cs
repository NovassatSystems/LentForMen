using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core
{
    [ContentProperty(nameof(Source))]
    public class EmbeddedResourceImageExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Source)) return null;

            var imageSource = $"Core.Images.{Source}.png";

            return ImageSource.FromResource(imageSource, GetType().Assembly);
        }
    }
}
