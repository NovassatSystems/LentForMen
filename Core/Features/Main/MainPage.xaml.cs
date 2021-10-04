using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Core
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            toShare.Command = new Command(async () =>
            {
                await Task.WhenAll(ChangeVisible(true));
                Device.BeginInvokeOnMainThread(() => { toPrint.ExecutePrint.Invoke(); });

            });
        }

        async Task ChangeVisible(bool value)
        {
            await Task.Delay(250);
            toFollow.IsVisible = value;
            toShare.IsVisible = Share.IsVisible = !value;
            await Task.FromResult(true);
        }
        protected override async void OnAppearing()
        {
            await Task.WhenAll(ChangeVisible(false));
            base.OnAppearing();
        }

        
    }
}