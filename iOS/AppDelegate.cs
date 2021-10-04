
using Core;
using CoreAnimation;
using CoreGraphics;
using FFImageLoading.Forms.Platform;
using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("Brush_Experimental", "Shapes_Experimental", "RadioButton_Experimental");
            global::Xamarin.Forms.Forms.Init();

            var navigationBarAppearace = UINavigationBar.Appearance;
            navigationBarAppearace.TintColor = Color.White.ToUIColor(); // Back buttons and such
            navigationBarAppearace.BarTintColor = Color.Transparent.ToUIColor();  // Bar's background color
            navigationBarAppearace.SetTitleTextAttributes(new UITextAttributes()
            {
                TextColor = Color.White.ToUIColor()
            });
            navigationBarAppearace.ShadowImage = new UIImage();

            LoadApplication(new App());
            KeyboardOverlapRenderer.Init();

            Plugin.LocalNotification.NotificationCenter.AskPermission();

            CachedImageRenderer.Init();

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }
    }
}
