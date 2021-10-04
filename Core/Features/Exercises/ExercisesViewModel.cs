using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ExercisesViewModel : BaseViewModel
    {
        private DelegateCommand _closeModalCommand;
        public DelegateCommand CloseModalCommand => _closeModalCommand ??
            (_closeModalCommand = new DelegateCommand(async () => await ExecuteCloseModalCommandAsync()));

        string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        public ExercisesViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                FileName = parameters.GetValue<string>("FileName");
               
            }
            base.OnNavigatedTo(parameters);
        }

        private async Task ExecuteCloseModalCommandAsync()
            => await _navigationService.GoBackAsync();
    }
}
