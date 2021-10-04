using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ExercisesViewModelB : BaseViewModel
    {
        private DelegateCommand _closeModalCommand;
        public DelegateCommand CloseModalCommand => _closeModalCommand ??
            (_closeModalCommand = new DelegateCommand(async () => await ExecuteCloseModalCommandAsync()));

        public ExercisesViewModelB(INavigationService navigationService) : base(navigationService)
        {

        }

        private async Task ExecuteCloseModalCommandAsync()
            => await _navigationService.GoBackAsync();
    }
}
