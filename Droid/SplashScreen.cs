using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Droid
{
    [Activity(Label = "Quaresma para Homens",
         Icon = "@drawable/ic_launcher",
         Theme = "@style/Theme.Splash",
         MainLauncher = true,
         NoHistory = true,
         ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : AndroidX.AppCompat.App.AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
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
    }
}
