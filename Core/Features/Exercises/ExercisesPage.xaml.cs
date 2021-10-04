using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core
{
    public partial class ExercisesPage : ContentPage
    {
        ExercisesViewModel ViewModel => BindingContext as ExercisesViewModel;
        string _id;
        public ExercisesPage()
        {
            InitializeComponent();
            LoadPdf();
        }

        protected override void OnAppearing()
        {
            _id = ViewModel.FileName;
            base.OnAppearing();
        }

        private async void LoadPdf()
        {
            var dependency = DependencyService.Get<ILocalFileProvider>();

            if (dependency == null)
            {
                await DisplayAlert("Erro ao carregar dependencia", "Dependencia não encontrada", "OK");

                return;
            }


            var file = SaveMediaHelper.GetLocal(App.PDFID);

            Stream stream = new MemoryStream(file);

            var localPath = string.Empty;

            //string url = "https://esposopaieprovedor.com.br/wp-content/uploads/2021/02/quaresma-semana-1.pdf";

            var fileName = Guid.NewGuid().ToString();

            //var assembly = typeof(App).GetTypeInfo().Assembly;

            //var stream = assembly.GetManifestResourceStream("Core.exercise.pdf");
            
            localPath = Task.Run(() => dependency.SaveFileToDisk(stream, $"{fileName}.pdf")).Result;
            
            if (string.IsNullOrWhiteSpace(localPath))
            {
                DisplayAlert("Error baixar PDF", "não foi possivel encontrar o arquivo", "OK");

                return;
            }

            PdfView.Uri = localPath;
        }

        
    }
}