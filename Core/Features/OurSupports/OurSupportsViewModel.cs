using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core
{
    public class OurSupportsViewModel: BaseViewModel
    {

        
        DelegateCommand<string> _openInstagram;
        public DelegateCommand<string> OpenInstagram => _openInstagram ??
            (_openInstagram = new DelegateCommand<string>(async(instaID) => await ExecuteOpenInstagramCommandAsync(instaID)));

        DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand => _closeCommand ??
            (_closeCommand = new DelegateCommand(async () => await ExecuteCloseCommandAsync()));

        string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        List<Supporter> _supporters;
        public List<Supporter> Supporters
        {
            get => _supporters;
            set => SetProperty(ref _supporters, value);
        }

        public OurSupportsViewModel(INavigationService navigationService) : base(navigationService)
        {
            
        }

        private async Task ExecuteCloseCommandAsync()
            => await _navigationService.GoBackAsync();

        private async Task ExecuteOpenInstagramCommandAsync(string instaID)
        {
            //try
            //{
            //    await Launcher.OpenAsync(new Uri($"instagram://user?username={instaID}"));
            //}
            //catch (Exception)
            //{
            //    var cfg = new ToastConfig($"Instagram não instalado no dispositivo")
            //    {
            //        Message = $"Instagram não instalado no dispositivo",
            //        Position = ToastPosition.Top,
            //        BackgroundColor = Color.FromHex("#7F64547a")
            //    };
            //    UserDialogs.Instance.Toast(cfg);
            //}
            //_audioPlayerService.PlayAudioFile("PrimeiroDia.mp3");
            var stream = GetStreamFromFile("_01.mp3");
            var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            audio.Load(stream);
            audio.Play();
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("Core.Audios." + filename);

            return stream;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            Supporters = OurSupportersHelper.GetSupporters();
            base.OnNavigatedTo(parameters);
        }
    }

    public static class OurSupportersHelper
    {
        public static List<Supporter> GetSupporters()
        {
            var supporters = new List<Supporter>
            {
                new Supporter
                {
                    Avatar = "alineBrasilPerfil",
                    UserName = "alinebrasiloficial",
                    Name = "Aline Brasil"
                },
                new Supporter   
                {
                    Avatar = "freiMarceloPerfil",
                    UserName = "frei_marcelo",
                    Name = "Frei Marcelo"
                },
                //new Supporter
                //{
                //    Avatar = "ic_launcher",
                //    UserName = "alinebrasiloficial",
                //    Name = "Aline Brasil 3"
                //},
                //new Supporter
                //{
                //    Avatar = "ic_launcher",
                //    UserName = "alinebrasiloficial",
                //    Name = "Aline Brasil 4"
                //},
                //new Supporter
                //{
                //    Avatar = "ic_launcher",
                //    UserName = "alinebrasiloficial",
                //    Name = "Aline Brasil 5"
                //},
                //new Supporter
                //{
                //    Avatar = "ic_launcher",
                //    UserName = "alinebrasiloficial",
                //    Name = "Aline Brasil 6"
                //}
            };

            supporters.Shuffle();
            return supporters;
        }
    }

    public class Supporter
    {
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
