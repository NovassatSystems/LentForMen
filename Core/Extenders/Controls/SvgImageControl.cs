using System;
using System.IO;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace Core
{
    public class SvgImageControl : Frame
    {
        #region Private Members

        private readonly SKCanvasView _canvasView = new SKCanvasView();

        #endregion

        #region Bindable Properties

        #region ResourceId

        public static readonly BindableProperty ResourceIdProperty = BindableProperty.Create(
            nameof(ResourceId), typeof(string), typeof(SvgImageControl), default(string), propertyChanged: RedrawCanvas);

        public string ResourceId
        {
            get => (string)GetValue(ResourceIdProperty);
            set => SetValue(ResourceIdProperty, value);
        }

        #endregion

        #region Size

        public static readonly BindableProperty WidthAndHeightProperty = BindableProperty.Create(
            nameof(WidthAndHeight), typeof(double), typeof(SvgImageControl), default(double),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is SvgImageControl self && newValue is double size)
                {
                    self.HeightRequest =
                    self.WidthRequest =
                    self.MinimumWidthRequest =
                    self.MinimumHeightRequest = size;
                }
            });

        public double WidthAndHeight
        {
            get => (double)GetValue(WidthAndHeightProperty);
            set => SetValue(WidthAndHeightProperty, value);
        }

        #endregion

        #region Command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(SvgImageControl), propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is SvgImageControl svgIcon && newValue is ICommand command)
                {
                    svgIcon.GestureRecognizers.Clear();
                    var tap = new TapGestureRecognizer
                    {
                        NumberOfTapsRequired = 1
                    };
                    tap.Tapped += (sender, args) =>
                    {
                        svgIcon?.Command?.Execute(svgIcon?.CommandParameter);
                    };
                    svgIcon.GestureRecognizers.Add(tap);
                }
            });

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion

        #region Command

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter), typeof(object), typeof(SvgImageControl));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        #endregion

        #region Constructor

        public SvgImageControl()
        {
            Padding = new Thickness(0);
            BackgroundColor = Color.Transparent;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            HasShadow = false;
            Content = _canvasView;
            _canvasView.PaintSurface += CanvasViewOnPaintSurface;
        }

        #endregion

        #region Private Methods

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            var svgIcon = bindable as SvgImageControl;
            svgIcon?._canvasView.InvalidateSurface();
        }

        private void CanvasViewOnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            if (string.IsNullOrEmpty(ResourceId))
                return;

            using (Stream stream = GetType().Assembly.GetManifestResourceStream($"Core.Images.{ResourceId}.svg"))
            {
                var svg = new SKSvg();
                svg.Load(stream);

                var info = args.Info;
                canvas.Translate(info.Width / 2f, info.Height / 2f);

                var bounds = svg.ViewBox;
                float xRatio = info.Width / bounds.Width;
                float yRatio = info.Height / bounds.Height;

                float ratio = Math.Min(xRatio, yRatio);

                canvas.Scale(ratio);
                canvas.Translate(-bounds.MidX, -bounds.MidY);

                canvas.DrawPicture(svg.Picture);
            }
        }

        #endregion
    }
}
