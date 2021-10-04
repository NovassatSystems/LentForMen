using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core
{

    public class ReflectionDetailViewModel : BaseViewModel
    {
        readonly ILoginService _loginService;

        public double CurrentPosition;

        bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value);
        }

        
       
        bool _showControls;
        public bool ShowControls
        {
            get => _showControls;
            set => SetProperty(ref _showControls, value);
        }
        
        bool _showOpenPDF;
        public bool ShowOpenPDF
        {
            get => _showOpenPDF;
            set => SetProperty(ref _showOpenPDF, value);
        }

        private DelegateCommand _addCommentCommand;
        public DelegateCommand AddCommentCommand => _addCommentCommand ??
            (_addCommentCommand = new DelegateCommand(async () => await ExecuteAddCommentCommandAsync()));

        private DelegateCommand _closeModalCommand;
        public DelegateCommand CloseModalCommand => _closeModalCommand ??
            (_closeModalCommand = new DelegateCommand(async () => await ExecuteCloseModalCommandAsync()));

        private DelegateCommand _playOrPauseAudioCommand;
        public DelegateCommand PlayOrPauseAudioCommand => _playOrPauseAudioCommand ??
            (_playOrPauseAudioCommand = new DelegateCommand(async () => await ExecutePlayOrPauseAudioCommand()));

        private DelegateCommand _stopAudioCommand;
        public DelegateCommand StopAudioCommand => _stopAudioCommand ??
            (_stopAudioCommand = new DelegateCommand(async () => await ExecuteStopAudioCommand()));

        private DelegateCommand _showExercisesCommand;
        public DelegateCommand ShowExercisesCommand => _showExercisesCommand ??
            (_showExercisesCommand = new DelegateCommand(async () => await ExecuteShowExercisesCommand()));

        private DelegateCommand _downloadAudioCommand;
        public DelegateCommand DownloadAudioCommand => _downloadAudioCommand ??
            (_downloadAudioCommand = new DelegateCommand(async () => await ExecuteDownloadAudioCommand()));

        private DelegateCommand _downloadPdfCommand;
        public DelegateCommand DownloadPdfCommand => _downloadPdfCommand ??
            (_downloadPdfCommand = new DelegateCommand(async () => await ExecuteDownloadPdfCommand()));

        private DelegateCommand _openInitialPrayerCommand;
        public DelegateCommand OpenInitialPrayerCommand => _openInitialPrayerCommand ?? 
            (_openInitialPrayerCommand = new DelegateCommand(async () => await ExecuteOpenInitialPrayerCommand()));

        ReflectionDayWrapper _item;
        public ReflectionDayWrapper Item { get => _item; set => SetProperty(ref _item, value); }

        int Index;
        public ReflectionDetailViewModel(INavigationService navigationService, ILoginService loginService) : base(navigationService)
        {
            _loginService = loginService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            App.CurrentPosition = 0;
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                Index = parameters.GetValue<int>("Index");
                Item = await LoadDataAsync();
                
                if (Item != null)
                {

                    var dataArray = SaveMediaHelper.GetLocal(Item.Id);
                    ShowControls = dataArray != null & dataArray?.Length > 0;
                    if (Item.WorkoutInfo?.Days != null)
                    {
                        var dataArrayPDF = SaveMediaHelper.GetLocal(Item.WorkoutInfo?.Days);
                        ShowOpenPDF = dataArrayPDF != null & dataArrayPDF?.Length > 0;
                    }
                        
                }
            }

            base.OnNavigatedTo(parameters);
        }

        public async void OnAppearing()
        {
            Item = await LoadDataAsync();
        }

        async Task ExecuteOpenInitialPrayerCommand()
        {
            UserDialogs.Instance.Alert(new AlertConfig
            {
                Message = "Inspirai, Senhor, as nossas ações e ajudai-nos a realizá-las, para que em Vós comece e para Vós termine tudo aquilo que fizermos. Por Cristo Senhor Nosso.",
                OkText = "Amém!",
                Title = "Oração inicial"
            });
        }

        async Task ExecuteDownloadAudioCommand()
        {
            string localPath = string.Empty;
            string url = "https://esposopaieprovedor.com.br/wp-content/uploads/2021/02/"+Item.Index+".mp3";
            UserDialogs.Instance.ShowLoading("Baixando arquivos de audio da reflexão do dia", MaskType.Gradient);
            var data = await DownloadAudioAsync(url);
            if (data != null && data?.Length > 0)
            {
                await SaveMediaHelper.SaveLocal(Item.Id, $"{Item.Index}.mp3", data);
                ShowControls = true;
            }
            else
            {
                var cfg = new ToastConfig($"Falha no download, tente novamente")
                {
                    Message = $"Falha no download, tente novamente",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#7F64547a")
                };
                UserDialogs.Instance.Toast(cfg);
            }

            UserDialogs.Instance.HideLoading();
        }

        async Task ExecuteDownloadPdfCommand()
        {
            ExerciciosModel workoutDay = new ExerciciosModel();
            var workoutDB = StoreService.Instance.GetCollection<ExerciciosModel>().Query().ToList();
            foreach (var item in workoutDB)
            {
                var splitted = new List<string>(item.Days.Split('-'));
                if (splitted.Contains(Index.ToString()))
                {
                    workoutDay = item;
                    break;
                }
            }


            string localPath = string.Empty;
            string url = $"{workoutDay.URL}";
            UserDialogs.Instance.ShowLoading("Baixando arquivo de PDF dos exercícios", MaskType.Gradient);
            var data = await DownloadAudioAsync(url);
            if (data != null && data?.Length > 0)
            {
                await SaveMediaHelper.SaveLocal(Item.WorkoutInfo.Days, $"{workoutDay.Days}.pdf", data);

                ShowOpenPDF = true;
            }
            else
            {
                var cfg = new ToastConfig($"Falha no download, tente novamente")
                {
                    Message = $"Falha no download, tente novamente",
                    Position = ToastPosition.Top,
                    BackgroundColor = Color.FromHex("#7F64547a")
                };
                UserDialogs.Instance.Toast(cfg);
            }

            UserDialogs.Instance.HideLoading();
        }

        async Task<byte[]> DownloadAudioAsync(string imageUrl)
        {
            var _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

            try
            {
                using (var httpResponse = await _httpClient.GetAsync(imageUrl))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var array = await httpResponse.Content.ReadAsByteArrayAsync();
                        return array;
                    }
                    else
                        return null;
                }
            }
            catch (Exception)
            {
                //Handle Exception
                return null;
            }
        }

        private async Task ExecuteShowExercisesCommand()
        {
            //if(Item?.TypeWorkout != null)
            //{
            //    switch (Item.TypeWorkout)
            //    {
            //        case TypeWorkout.Default:
            //            await _navigationService.NavigateAsync(nameof(ExercisesPage), null, true, true);
            //            break;
            //        case TypeWorkout.WorkoutA:
            //            await _navigationService.NavigateAsync(nameof(ExercisesPageA), null, true, true);
            //            break;
            //        case TypeWorkout.WorkoutB:
            //            await _navigationService.NavigateAsync(nameof(ExercisesPageB), null, true, true);
            //            break;
            //        default:
            //            break;
            //    }
            //}
            var parms = new NavigationParameters();
            parms.Add("FileName", Item.WorkoutInfo.Days);
            App.PDFID = Item.WorkoutInfo.Days;
            await _navigationService.NavigateAsync(nameof(ExercisesPage), parms, true, true);

        }



        public async Task ExecutePlayOrPauseAudioCommand()
        {


            var audio = CrossSimpleAudioPlayer.Current;
            if (audio.IsPlaying)
            {
                audio.Pause();
                App.CurrentPosition = audio.CurrentPosition;
            }

            else
            {
                if (App.CurrentPosition > 0)
                    audio.Seek(App.CurrentPosition);
                else
                {

                    var data = SaveMediaHelper.GetLocal(Item.Id);

                    if (data != null && data?.Length > 0)
                    {
                        var stream = new MemoryStream(data);
                        if (stream == null)
                        {
                            var cfg = new ToastConfig($"Arquivo de aúdio não encontrado")
                            {
                                Message = $"Arquivo de aúdio não encontrado",
                                Position = ToastPosition.Top,
                                BackgroundColor = Color.FromHex("#7F64547a")
                            };
                            UserDialogs.Instance.Toast(cfg);
                            return;
                        }
                        audio.Load(stream);
                    }
                }
                audio.Play();
            }

            IsPlaying = audio.IsPlaying;
        }

        public async Task ExecuteStopAudioCommand()
        {
            IsPlaying = false;
            App.CurrentPosition = 0;
            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Stop();
        }

        public async Task ExecutePauseAudioCommand()
        {
            var audio = CrossSimpleAudioPlayer.Current;
            audio.Pause();
        }

        
        private async Task ExecuteAddCommentCommandAsync()
        {
            var parameter = new NavigationParameters();
            parameter.Add("IndexAnnotation", Index);
            await _navigationService.NavigateAsync(nameof(AddCommentPage), parameter, true, true);
        }
        private async Task ExecuteCloseModalCommandAsync()
        {
            CrossSimpleAudioPlayer.Current.Stop();
            App.CurrentPosition = 0;
            await _navigationService.GoBackAsync();
        }
        protected async Task<ReflectionDayWrapper> LoadDataAsync()
            => await ReflectionsService.Get(Index);

        //public override async Task InitializeAsync(object navigationData)
        //{
        //    if (navigationData is int index)
        //        Index = index;
        //    if (navigationData is bool annotationAdded && annotationAdded)
        //        await RefreshDataAsync();
        //    await base.InitializeAsync(navigationData);
        //}
    }
}
