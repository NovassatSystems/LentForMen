using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using Core;
using FFImageLoading.Forms.Platform;
using Plugin.LocalNotification;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Droid
{
    [Activity(Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleTop,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public static StatusBarVisibility DefaultSystemUiVisibility { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            //StoreService.Start();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            NotificationCenter.CreateNotificationChannel(
                new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest
                {
                    Sound = Resource.Raw.bell.ToString()
                });
            Forms.SetFlags("Brush_Experimental", "Shapes_Experimental", "RadioButton_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            TransparentStatusBar();
            LoadApplication(new App());
            NotificationCenter.NotifyNotificationTapped(Intent);
            CachedImageRenderer.Init(true);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

        }

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }

        private void TransparentStatusBar()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                // for covering the full screen in android..
                Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

                // clear FLAG_TRANSLUCENT_STATUS flag:
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);

                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                Window.DecorView.SystemUiVisibility = MainActivity.DefaultSystemUiVisibility & (StatusBarVisibility)SystemUiFlags.LightStatusBar;

            }

        }

        public override Android.Views.View OnCreateView(Android.Views.View parent, string name, Context context, IAttributeSet attrs)
        {
            return base.OnCreateView(parent, name, context, attrs);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}