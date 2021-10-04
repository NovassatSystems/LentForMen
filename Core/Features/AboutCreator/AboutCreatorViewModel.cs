using Acr.UserDialogs;
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
    public class AboutCreatorViewModel : BaseViewModel
    {

        DelegateCommand _changeNameCommand;
        public DelegateCommand ChangeNameCommand => _changeNameCommand ??
            (_changeNameCommand = new DelegateCommand(() => ExecuteChangeNameCommandAsync()));

        DelegateCommand _changeHourCommand;
        public DelegateCommand ChangeHourCommand => _changeHourCommand ??
            (_changeHourCommand = new DelegateCommand(() => ExecuteChangeHourCommandAsync()));

        DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand => _closeCommand ??
            (_closeCommand = new DelegateCommand(async () => await ExecuteCloseCommandAsync()));

        string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        TimeSpan _notificationHour;
        public TimeSpan NotificationHour { get => _notificationHour; set => SetProperty(ref _notificationHour, value); }
        public AboutCreatorViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
        private async Task ExecuteCloseCommandAsync()
            => await _navigationService.GoBackAsync();

        private void ExecuteChangeNameCommandAsync()
        {
            Preferences.Set("UserName", Name);
            var cfg = new ToastConfig($"Nome alterado para{Name}")
            {
                Message = $"Nome alterado para{Name}",
                Position = ToastPosition.Top,
                BackgroundColor = Color.FromHex("#64547a")
            };
            UserDialogs.Instance.Toast(cfg);
        }

        private void ExecuteChangeHourCommandAsync()
        {
            Preferences.Set("NotificationHour", NotificationHour.ToString());
            var cfg = new ToastConfig($"Horário das notificações alterado para{NotificationHour}")
            {
                Message = $"Horário das notificações alterado para{NotificationHour}",
                Position = ToastPosition.Top,
                BackgroundColor = Color.FromHex("#64547A")
    };
            UserDialogs.Instance.Toast(cfg);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            Name = Preferences.Get("UserName", string.Empty);

            TimeSpan DefaultHour = new TimeSpan(8, 0, 0);
            TimeSpan SavedHour;
            var timeString = Preferences.Get("NotificationHour", DefaultHour.ToString());
            if (TimeSpan.TryParse(timeString, out SavedHour))
                NotificationHour = SavedHour;
            else
                NotificationHour = DefaultHour;

            ReflectionsService.Update();
            base.OnNavigatedTo(parameters);
        }
    }
}
