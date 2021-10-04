using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Autem.Droid.Rendenres;
using Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(TransitionNavigationPageRenderer))]
namespace Autem.Droid.Rendenres
{
    public class TransitionNavigationPageRenderer : NavigationPageRenderer
    {
        public TransitionNavigationPageRenderer(Context context) : base(context)
        {
        }

        protected override void SetupPageTransition(AndroidX.Fragment.App.FragmentTransaction transaction, bool isPush)
        {
            if (isPush)
            {
                transaction.SetCustomAnimations(Resource.Animation.enter_right, Resource.Animation.exit_left,
                                                Resource.Animation.enter_left, Resource.Animation.exit_right);
            }
            else
            {
                transaction.SetCustomAnimations(Resource.Animation.enter_left, Resource.Animation.exit_right,
                                                Resource.Animation.enter_right, Resource.Animation.exit_left);
            }
        }
    }
}
