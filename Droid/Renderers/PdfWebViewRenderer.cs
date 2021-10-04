using System;
using Android.Content;
using Core;
using Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfWebViewRenderer))]
namespace Droid
{
    public class PdfWebViewRenderer : WebViewRenderer
    {
        public PdfWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as PdfWebView;

                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;

                if (!string.IsNullOrEmpty(customWebView.Uri))
                {
                    Control.LoadUrl($"file:///android_asset/pdfjs/web/viewer.html?file={System.Net.WebUtility.UrlEncode(customWebView.Uri)}");
                }
            }
        }
    }
}