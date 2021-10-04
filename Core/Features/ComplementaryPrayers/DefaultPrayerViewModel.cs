using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace Core
{
    public class DefaultPrayerViewModel : BaseViewModel
    {
        private DelegateCommand _closeModalCommand;
        public DelegateCommand CloseModalCommand => _closeModalCommand ??
            (_closeModalCommand = new DelegateCommand(async () => await ExecuteCloseModalCommandAsync()));
        public DefaultPrayerViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private async Task ExecuteCloseModalCommandAsync()
           => await _navigationService.GoBackAsync();
    }
}
