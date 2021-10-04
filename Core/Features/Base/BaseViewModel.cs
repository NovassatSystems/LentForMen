using Prism.Mvvm;
using Prism.Navigation;

namespace Core
{
    public abstract class BaseViewModel : BindableBase, INavigationAware, IDestructible
    {
        protected readonly INavigationService _navigationService;

        private string _title;
        public string Title
        {
            get => _title;
            protected set => SetProperty(ref _title, value);
        }

        protected BaseViewModel(INavigationService navigationService)
            => _navigationService = navigationService;

        protected BaseViewModel(INavigationService navigationService, string title) : this(navigationService)
            => Title = title;

        
        //Quando você estiver saindo da página .. e antes de navegar dela ..        
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        //Será disparado -invoked- quando você chegar à página.
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }
    }
}
