using System;
using Autem.iOS.Rendenres;
using Core;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PrintViewControl), typeof(PrintViewControlRenderer))]
namespace Autem.iOS.Rendenres
{
    public class PrintViewControlRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
                return;

            (Element as PrintViewControl).ExecutePrint = new Action(() =>
            {
                var imageArray = ExecutePrint();
                (Element as PrintViewControl).OnPrintCompleted.Invoke(imageArray);
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (Element is PrintViewControl e)
                e.ExecutePrint = null;

            base.Dispose(disposing);
        }

        private byte[] ExecutePrint()
        {
            UIGraphics.BeginImageContextWithOptions(this.Bounds.Size, opaque: true, scale: 0);
            UIImage image;

            this.DrawViewHierarchy(this.Bounds, afterScreenUpdates: true);
            image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (NSData imageData = image.AsPNG())
            {
                var myByteArray = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                return myByteArray;
            }
        }
    }
}