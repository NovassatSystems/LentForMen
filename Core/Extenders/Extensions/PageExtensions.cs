using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Core
{
    public static class PageExtensions
    {
        public static void HideNavigationBar(this Page self)
        {
            NavigationPage.SetHasBackButton(self, false);
            NavigationPage.SetHasNavigationBar(self, false);
            NavigationPage.SetBackButtonTitle(self, string.Empty);
        }

        public static void WindowSoftInputModeAdjustResize(this Page self)
           => App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

        public static void WindowSoftInputModeAdjustPan(this Page self)
           => App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

      
    }
}
