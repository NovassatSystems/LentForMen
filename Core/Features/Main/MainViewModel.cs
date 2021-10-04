using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Net.Http;
using System.Net;
using System.IO;

namespace Core
{
    public class MainViewModel : BaseViewModel
    {
        readonly ILoginService _loginService;

        #region Commands
        private DelegateCommand<int?> _openDayCommand;
        public DelegateCommand<int?> OpenDayCommand => _openDayCommand ??
            (_openDayCommand = new DelegateCommand<int?>(async (result) => await ExecuteOpenDayCommandAsync(result)));

        DelegateCommand _openMenuCommand;
        public DelegateCommand OpenMenuCommand => _openMenuCommand ??
            (_openMenuCommand = new DelegateCommand(async () => await ExecuteOpenMenuCommandAsync()));

        private DelegateCommand _registerUserCommand;
        public DelegateCommand RegisterUserCommand => _registerUserCommand ??
            (_registerUserCommand = new DelegateCommand(async () => await ExecuteRegisterUserCommand()));

      

        #endregion

        #region Properties
        int _dayIndex;
        public int DayIndex { get => _dayIndex; set => SetProperty(ref _dayIndex, value); }
        string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        bool _stayLogged;
        public bool StayLogged
        {
            get => _stayLogged;
            set => SetProperty(ref _stayLogged, value);
        }

        string _fraseInicial;
        public string FraseInicial
        {
            get => _fraseInicial;
            set => SetProperty(ref _fraseInicial, value);
        }

        string _imageBackground;
        public string ImageBackground
        {
            get => _imageBackground;
            set => SetProperty(ref _imageBackground, value);
        }


        List<ReflectionDayWrapper> _reflections;
        public List<ReflectionDayWrapper> Reflections { get => _reflections; set => SetProperty(ref _reflections, value); }
        #endregion

        #region Ctor
        public MainViewModel(INavigationService navigationService, ILoginService loginService) : base(navigationService)
        {
            _loginService = loginService;
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region ExecuteCommands
       

        private async Task ExecuteRegisterUserCommand()
        {
            bool registered;
            if (!(await _loginService.IsUserExist(Username)))
            {
                registered = await _loginService.RegisterUser(new UserModel
                {
                    Username = Username,
                    Password = Password,
                    DeviceModel = "zero",
                    Reset = false
                });
                if (registered)
                {
                    var cfg = new ToastConfig($"Usuário registrado")
                    {
                        Message = $"Usuário registrado",
                        Position = ToastPosition.Top,
                        BackgroundColor = Color.FromHex("#7F64547a")
                    };
                    UserDialogs.Instance.Toast(cfg);
                    return;
                }
                var cfgError = new ToastConfig($"Ocorreu um erro")
                {
                    Message = $"Ocorreu um erro",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#64547a")
                };
                UserDialogs.Instance.Toast(cfgError);

            }
            else
            {
                var cfg = new ToastConfig($"Usuário ja registrado")
                {
                    Message = $"Usuário ja registrado",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#64547a")
                };
                UserDialogs.Instance.Toast(cfg);
            }

        }

        async Task ExecuteOpenDayCommandAsync(int? index)
        {
            if (index == -1)
            {
                var cfg = new ToastConfig($"A quaresma ainda não iniciou")
                {
                    Message = $"A quaresma ainda não iniciou",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#7F64547a")
                };
                UserDialogs.Instance.Toast(cfg);
                return;
            }
            var parameter = new NavigationParameters();
            parameter.Add("Index", index);
            await _navigationService.NavigateAsync(nameof(ReflectionDetailPage), parameter, true, true);
        }

        private async Task ExecuteOpenMenuCommandAsync()
            => await _navigationService.NavigateAsync(nameof(MenuPage), null, true, true);
        #endregion

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadDataAsync();
            var objectInicial = await _loginService.GetFraseInicial();
            FraseInicial = !string.IsNullOrEmpty(objectInicial.Description) ? objectInicial.Description : "\"O estudioso é aquele que leva aos demais o que ele compreendeu: a Verdade\"";
            ImageBackground = !string.IsNullOrEmpty(objectInicial.ImageBackground) ? objectInicial.ImageBackground : "ic_background_home";
            //if(parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.New)
            //{
            //    if (parameters.GetValue<bool>("ChangePwd"))
            //    {
            //        await _navigationService.
            //    }
            //}


            Name = Preferences.Get("UserName", string.Empty);
            var list = StoreService.Instance.GetCollection<ReflectionDayModel>();

            var days = list.Query().ToList();

            DayIndex = list.Query().ToList().Find(f => f.DayofYear.Date.Day == DateTime.Today.Day &&
            f.DayofYear.Month == DateTime.Today.Month)?.Index ?? -1;

            if (parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.New)
            {
                 
                
            }

            

        }

        async Task LoadDataAsync()
        {
            Reflections = await ReflectionsService.GetList();
        }


    }

    public static class SaveMediaHelper
    {
        public static async Task SaveLocal(string mediaId, string name, byte[] media)
        {
            using (MemoryStream stream = new MemoryStream(media))
            {
                await Task.Run(() =>
                {
                    var result = StoreService.Instance.FileStorage.Upload(mediaId, name, stream);
                });
            }
        }

        public static byte[] GetLocal(string mediaId)
        {
            var existFile = StoreService.Instance.FileStorage.Exists(mediaId);
            if (existFile)
            {

                var stream = StoreService.Instance.FileStorage.OpenRead(mediaId);
                using (stream)
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        stream.CopyTo(memStream);
                        return memStream.ToArray();
                    }
                }
            }
            return null;
        }
    }
}
