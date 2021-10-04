using Autem.iOS.Rendenres;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Autem.iOS.Rendenres
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            // remove the border form the entry
            if (Control != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
                Element.HeightRequest = 40;
            }
        }
    }
}