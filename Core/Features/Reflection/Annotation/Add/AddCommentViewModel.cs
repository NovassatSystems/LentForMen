using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core
{
    public class AddCommentViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        int _id;
        string _annotation;
        public string Annotation
        {
            get => _annotation;
            set => SetProperty(ref _annotation, value);
        }

        public AddCommentViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveCommand = new Command(async() => await ExecuteSaveCommandAsync());
            CancelCommand = new Command(async() => await ExecuteCancelCommandAsync());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.GetNavigationMode() == NavigationMode.New)
            {
                _id = parameters.GetValue<int>("IndexAnnotation");

            }
            base.OnNavigatedFrom(parameters);
        }


        private async Task ExecuteCancelCommandAsync()
            => await _navigationService.GoBackAsync();
        private async Task ExecuteSaveCommandAsync()
        {
            var reflections = StoreService.Instance.GetCollection<ReflectionDayModel>();

            reflections.EnsureIndex(m => m.Id);
            reflections.EnsureIndex(m => m.Index);

            var reflection = reflections.Query().Where(o => o.Index == _id).ToList();
            if (reflection == null)
                return;

            if (reflection[0].Annotations == null)
                reflection[0].Annotations = new List<AnnotationModel>();

            reflection[0].Annotations.Add(new AnnotationModel { Description = Annotation, DateTime = DateTime.UtcNow.ToLocalTime()});
            StoreService.Instance.GetCollection<ReflectionDayModel>().Upsert(reflection);

            var parameter = new NavigationParameters();
            parameter.Add("AnnotationAdded", true);
            await _navigationService.GoBackAsync(parameter);
        }
    }
}
