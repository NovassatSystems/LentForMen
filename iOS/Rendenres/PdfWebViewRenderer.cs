using Core;
using Foundation;
using iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfWebViewRenderer))]
namespace iOS
{
    public class PdfWebViewRenderer : ViewRenderer<PdfWebView, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PdfWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                // Cleanup
            }
            if (e.NewElement != null)
            {
                var customWebView = Element as PdfWebView;
                string fileName = customWebView.Uri;

                if (!string.IsNullOrEmpty(fileName))
                {
                    Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
                    Control.ScalesPageToFit = true;
                }
            }
        }
    }
}