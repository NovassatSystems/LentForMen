using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Core
{
    public class PdfWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create("Uri", typeof(string), typeof(PdfWebView), default(string));
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}
