using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Core;
using Droid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PrintViewControl), typeof(PrintViewControlRenderer))]
namespace Droid
{
    public class PrintViewControlRenderer : ViewRenderer<PrintViewControl, Android.Views.View>
    {
        public PrintViewControlRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<PrintViewControl> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            Element.ExecutePrint = new Action(() =>
            {
                if (ViewGroup == null)
                    return;
                var imageArray = ExecutePrint();
                Element.OnPrintCompleted.Invoke(imageArray);
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (Element != null)
                Element.ExecutePrint = null;

            base.Dispose(disposing);
        }

        private byte[] ExecutePrint()
        {
            //get the subview
            Android.Views.View subView = ViewGroup.GetChildAt(0);
            int width = subView.Width;
            int height = subView.Height;

            //create and draw the bitmap
            Bitmap b = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas c = new Canvas(b);
            ViewGroup.Draw(c);

            //save the bitmap to file
            MemoryStream ms = new MemoryStream();
            b.Compress(Bitmap.CompressFormat.Png, 100, ms);
            return ms.ToArray();
        }
    }
}