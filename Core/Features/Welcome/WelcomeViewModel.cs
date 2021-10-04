using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core
{
    public class WelcomeViewModel : BaseViewModel
    {

        DelegateCommand _nextCommand;
        public DelegateCommand NextCommand => _nextCommand ??
            (_nextCommand = new DelegateCommand(async () => await ExecuteNextCommandAsync()));
        string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public WelcomeViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private async Task ExecuteNextCommandAsync()
        {
            Preferences.Set("UserName", Name);
            await _navigationService.NavigateAsync(nameof(MainPage));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ReflectionsService.Update();
            base.OnNavigatedTo(parameters);
        }
    }
}
