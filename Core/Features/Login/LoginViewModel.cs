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
using Microsoft.AppCenter.Crashes;

namespace Core
{
    public class LoginViewModel : BaseViewModel
    {
        readonly ILoginService _loginService;

        #region Commands


        private DelegateCommand _registerUserCommand;
        public DelegateCommand RegisterUserCommand => _registerUserCommand ??
            (_registerUserCommand = new DelegateCommand(async () => await ExecuteInsertWorkouts()));

        private DelegateCommand _loginUserCommand;
        public DelegateCommand LoginUserCommand => _loginUserCommand ??
            (_loginUserCommand = new DelegateCommand(async () => await ExecuteLoginUserCommand()));

        private DelegateCommand _helpCommand;
        public DelegateCommand HelpCommand => _helpCommand ??
            (_helpCommand = new DelegateCommand(async () => await ExecuteHelpCommand()));

        private DelegateCommand _registerNewUsers;
        public DelegateCommand RegisterNewUsers => _registerNewUsers ??
            (_registerNewUsers = new DelegateCommand(async () => await ExecuteHelpCommand()));

        private DelegateCommand _stayLoggedCommand;
        public DelegateCommand StayLoggedCommand => _stayLoggedCommand ??
            (_stayLoggedCommand = new DelegateCommand(() => ExecuteStayLoggedCommand()));

        #endregion

        #region Properties
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

        string _version;
        public string Version 
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        List<ReflectionDayWrapper> _reflections;
        public List<ReflectionDayWrapper> Reflections { get => _reflections; set => SetProperty(ref _reflections, value); }
        #endregion

        #region Ctor
        public LoginViewModel(INavigationService navigationService, ILoginService loginService) : base(navigationService)
        {
            _loginService = loginService;
#if DEBUG
            Username = "Peter";
            Password = "123qwe";
#endif
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region ExecuteCommands
        private void ExecuteStayLoggedCommand() => StayLogged = !StayLogged;

        private async Task ExecuteLoginUserCommand()
        {
            UserDialogs.Instance.ShowLoading("Entrando", MaskType.Black);
            if ((string.IsNullOrEmpty(Username) || (string.IsNullOrEmpty(Password))))
            {
                var cfg = new ToastConfig($"O Usuário/Senha não está preenchido")
                {
                    Message = $"O Usuário/Senha não está preenchido",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#64547a")
                };
                UserDialogs.Instance.Toast(cfg);
                return;
            }

            var userModel = new UserModel();
            try
            {
                userModel = new UserModel { Username = Username, Password = Password, DeviceModel = DeviceHelper.GetDeviceModel(), Reset = false };
            }
            catch (Exception e )
            {
                Crashes.TrackError(e);
                userModel = new UserModel { Username = Username, Password = Password, DeviceModel = "LentForMen", Reset = false };
            }
            

            

            var user = await _loginService.LoginUser(Username, Password);

            if (user != null)
            {

                switch (user.Username)
                {
                    case "CALVINO":
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Message = "Parece que você não está conectado, verifique seu acesso à Internet",
                            OkText = "Ok",
                            Title = "Ho-ho"
                        });
                        return;
                    case "LUTERO":
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(new AlertConfig
                        {
                            OnAction = async () => await ExecuteHelpCommand(),
                            Message = "Problemas ao fazer login? Clique em \"Preciso de ajuda\" e converse com nosso suporte",
                            OkText = "Preciso de ajuda",
                            Title = "Ho-ho"
                        });
                        break;
                    case "CAMPANHAFRATERNIDADE":
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(new AlertConfig
                        {
                            OnAction = async () => await ExecuteHelpCommand(),
                            Message = "Você já realizou o acesso em outro dispositivo. Clique em \"Resetar acesso\" e converse com nosso suporte para requerir o reset do acesso.",
                            OkText = "Resetar acesso",
                            Title = "Ops!"
                        });
                        break;
                    case "FEMINISTA":
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(new AlertConfig
                        {
                            OnAction = async () => await ExecuteHelpCommand(),
                            Message = "Por favor contate o nosso suporte para autorizar seu acesso.",
                            OkText = "Suporte",
                            Title = "Erro no dispositivo"
                        });
                        break;
                    default:
                        //var parms = new NavigationParameters();

                        //if (userModel.DeviceModel == "zero")
                        //{
                        //    await UserDialogs.Instance.AlertAsync("Olá seja muito bem vindo ao programa \"Quaresma para Homens\". Para o seu primeiro acesso a senha foi gerada aleatoriamente, caso deseje permanecer com esta mesma senha, pedidos que anote ela em algum lugar seguro e não compartilhe com ninguém e na próxima tela clique em \"Não quero alterar\", caso queira insira uma senha com pelo menos 8 caracteres.",
                        //        "Salve Maria", "Ok, vamos lá!");
                        //    parms.Add("ChangePwd", true);
                        //}
                        Preferences.Set("Logged", StayLogged);
                        var parms = new NavigationParameters();
                        parms.Add("VerifyReset", true);
                        await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(Core.MainPage)}", parms, false, true);
                        return;

                }
            }

        }


        private async Task ExecuteInsertWorkouts() => await _loginService.RegisterWorkouts();
        private async Task ExecuteAdminCommand()
        {
            //await _navigationService.NavigateAsync(nameof(AdminPage));
            UserDialogs.Instance.ShowLoading($"Registrando {Username}");
            bool registered;
            if (!(await _loginService.IsUserExist(Username)))
            {
                var pwd = RandomPwd();
                registered = await _loginService.RegisterUser(new UserModel
                {
                    Username = Username,
                    DeviceModel = "zero",
                    Reset = false,
                    Password = pwd
                });



                if (registered)
                {
                    Password = pwd;
                    var cfg = new ToastConfig($"Usuário registrado")
                    {
                        Message = $"Usuário registrado",
                        Position = ToastPosition.Top,
                        BackgroundColor = Color.FromHex("#64547a")
                    };
                    UserDialogs.Instance.Toast(cfg);
                    await UserDialogs.Instance.AlertAsync($"{Username} foi registrado com a senha {Password}", "Usuário registrado", "Fechar", null);
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                var cfgError = new ToastConfig($"Ocorreu um erro")
                {
                    Message = $"Ocorreu um erro",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#7F64547a")
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
            UserDialogs.Instance.HideLoading();

        }

        async Task ExecuteHelpCommand()
        {
            var uri = "whatsapp://send?phone=+5541997597091&text=Olá, preciso de ajuda com o meu acesso dentro do app.";
            var canOpen = await Launcher.CanOpenAsync(uri);
            var device = Device.RuntimePlatform == Device.iOS ? "iPhone" : "smartphone";
            if (canOpen)
                await Launcher.OpenAsync(uri);
            else
                UserDialogs.Instance.Alert($"É necessário ter o Whatsapp instalado em seu {device} para falar com um de nossos suportes");
        }

       
        #endregion

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            Version = $"Version {VersionTracking.CurrentVersion}";
            if (parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.New)
            {
                var teste = RandomPwd();
                if (parameters.GetValue<bool>("Reseted"))
                    UserDialogs.Instance.Alert(new AlertConfig
                    {
                        OnAction = async () => await ExecuteHelpCommand(),
                        Message = "Um reset foi requisitado, você terá de realizar o login novamente. Se não foi você quem requisitou, por favor, clique em \"Contatar suporte\" e reporte a situação, fique tranquilo, você não perdeu o acesso.",
                        OkText = "Contatar suporte",
                        Title = "Reset requisitado"
                    });
            }
            await LoadDataAsync();
        }

        async Task LoadDataAsync()
        {
            Reflections = await ReflectionsService.GetList();
        }

        public string RandomPwd()
        {
            var t = Guid.NewGuid().ToString("N").Substring(1, 8);
            return t;
        }
    }
}
