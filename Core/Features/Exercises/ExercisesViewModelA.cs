using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ExercisesViewModelA : BaseViewModel
    {
        private DelegateCommand _closeModalCommand;
        public DelegateCommand CloseModalCommand => _closeModalCommand ??
            (_closeModalCommand = new DelegateCommand(async () => await ExecuteCloseModalCommandAsync()));

        public ExercisesViewModelA(INavigationService navigationService) : base(navigationService)
        {

        }

        private async Task ExecuteCloseModalCommandAsync()
            => await _navigationService.GoBackAsync();
    }
}
