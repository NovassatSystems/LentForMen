using System.Globalization;
using System.Threading;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.LocalNotification;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core
{
    public partial class App : PrismApplication
    {
        public static string PDFID;
        public App() : this(null)
        { }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;
            NotificationCenter.Current.NotificationReceived += OnLocalNotificationReceived;
#if !DEBUG
            RegisterAppCenter();
#endif
        }

        private void OnLocalNotificationReceived(NotificationReceivedEventArgs e)
        {
            var teste = e.Data;
        }

        private void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        {
            var teste = e.Data;
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            Xamarin.Forms.Device.SetFlags(new string[] { "MediaElement_Experimental" });
            ReflectionsService.Load();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(Core.MainPage)}", null, false, true);

        }

        public static double CurrentPosition;

        protected override void OnStart()
        {
            base.OnStart();
            var ptbrCulture = new CultureInfo("pt-br");
            Thread.CurrentThread.CurrentCulture = ptbrCulture;
            Thread.CurrentThread.CurrentUICulture = ptbrCulture;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navegacao
            containerRegistry.RegisterForNavigation<NavigationPage>();


            //Paginas iniciais
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<WelcomePage, WelcomeViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuViewModel>();
            containerRegistry.RegisterForNavigation<ReflectionDetailPage, ReflectionDetailViewModel>();
            containerRegistry.RegisterForNavigation<AddCommentPage, AddCommentViewModel>();
            containerRegistry.RegisterForNavigation<PreferencesPage, PreferencesViewModel>();
            containerRegistry.RegisterForNavigation<AboutProgramPage, AboutProgramViewModel>();
            containerRegistry.RegisterForNavigation<AboutCreatorPage, AboutCreatorViewModel>();
            containerRegistry.RegisterForNavigation<OurSupportsPage, OurSupportsViewModel>();
            containerRegistry.RegisterForNavigation<ExercisesPage, ExercisesViewModel>();
            containerRegistry.RegisterForNavigation<ExercisesPageA, ExercisesViewModelA>();
            containerRegistry.RegisterForNavigation<ExercisesPageB, ExercisesViewModelB>();
            containerRegistry.RegisterForNavigation<DefaultPrayerPage, DefaultPrayerViewModel>();

            containerRegistry.Register<ILoginService, LoginService>();

        }

        void RegisterAppCenter()
        {
            AppCenter.Start("3314a8df-4f89-4fa9-89bd-effa20cb3c72", typeof(Analytics), typeof(Crashes));
        }
    }
}
