using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core
{
    public class MenuViewModel : BaseViewModel
    {
        private DelegateCommand _closeModalCommand;
        public DelegateCommand CloseModalCommand => _closeModalCommand ??
            (_closeModalCommand = new DelegateCommand(async () => await ExecuteCloseModalCommandAsync()));

        private DelegateCommand _preferencesCommand;
        public DelegateCommand PreferencesCommand => _preferencesCommand ??
            (_preferencesCommand = new DelegateCommand(async () => await ExecutePreferencesCommandAsync()));

        private DelegateCommand _aboutProgram;
        public DelegateCommand AboutProgramCommand => _aboutProgram ??
            (_aboutProgram = new DelegateCommand(async () => await ExecuteAboutProgramCommandAsync()));

      

        private DelegateCommand _aboutCreator;
        public DelegateCommand AboutCreatorCommand => _aboutCreator ??
            (_aboutCreator = new DelegateCommand(async () => await ExecuteAboutCreatorCommandAsync()));

        private DelegateCommand _ourSupports;
        public DelegateCommand OurSupportsCommand => _ourSupports ??
            (_ourSupports = new DelegateCommand(async () => await ExecuteOurSupportsCommandAsync()));

        string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }



        public MenuViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Version = $"Version{VersionTracking.CurrentVersion}";
            base.OnNavigatedTo(parameters);
        }

        private async Task ExecutePreferencesCommandAsync()
            => await _navigationService.NavigateAsync(nameof(PreferencesPage), null, true, true);

        private async Task ExecuteAboutProgramCommandAsync()
            => await _navigationService.NavigateAsync(nameof(AboutProgramPage), null, true, true);

        private async Task ExecuteAboutCreatorCommandAsync()
            => await _navigationService.NavigateAsync(nameof(AboutCreatorPage), null, true, true);

        private async Task ExecuteOurSupportsCommandAsync()
        {
            //var cfg = new ToastConfig($"Função não implementada")
            //{
            //    Message = $"Função não implementada",
            //    Position = ToastPosition.Top,
            //    BackgroundColor = Color.FromHex("#7F64547a")
            //};
            //UserDialogs.Instance.Toast(cfg);
            await _navigationService.NavigateAsync(nameof(DefaultPrayerPage), null, true, true);
        }

        private async Task ExecuteCloseModalCommandAsync()
            => await _navigationService.GoBackAsync();

    }
}
