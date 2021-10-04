using System;
using System.Reflection;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Core
{
    public class SignatureControl : Frame
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(SignatureControl), default(string), propertyChanged: RedrawCanvas);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private readonly SKCanvasView _canvasView = new SKCanvasView();

        public SignatureControl()
        {
            Content = _canvasView;
            Padding = new Thickness(0);
            BackgroundColor = Color.Transparent;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HasShadow = false;
            _canvasView.PaintSurface += CanvasViewOnPaintSurface;
        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            var svgIcon = bindable as SignatureControl;
            svgIcon?._canvasView.InvalidateSurface();
        }

        private void CanvasViewOnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Black
            };

            textPaint.Typeface = GetTypeface();

            canvas.Translate(info.Width / 2f, info.Height / 2f);


            // Adjust TextSize property so text is 95% of screen width
            float textWidth = textPaint.MeasureText(Text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;

            // Find the text bounds
            var textBounds = new SKRect();
            textPaint.MeasureText(Text, ref textBounds);

            float xRatio = info.Width / textBounds.Width;
            float yRatio = info.Height / textBounds.Height;

            float ratio = Math.Min(xRatio, yRatio);

            canvas.Scale(ratio);
            canvas.Translate(-textBounds.MidX, -textBounds.MidY);

            // And draw the text
            canvas.DrawText(Text, xRatio, yRatio, textPaint);

        }

        public SKTypeface GetTypeface()
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Core.Font.Caveat-Bold.ttf");
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }
    }
}
