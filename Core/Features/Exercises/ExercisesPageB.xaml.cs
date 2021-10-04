using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core
{
    public partial class ExercisesPageB : ContentPage
    {
        public ExercisesPageB()
        {
            InitializeComponent();
            LoadPdf();
        }

        private void LoadPdf()
        {
            var dependency = DependencyService.Get<ILocalFileProvider>();

            if (dependency == null)
            {
                DisplayAlert("Erro ao carregar dependencia", "Dependencia não encontrada", "OK");

                return;
            }

            var localPath = string.Empty;

            string url = "https://esposopaieprovedor.com.br/wp-content/uploads/2021/02/quaresma-semana-1.pdf";

            var fileName = Guid.NewGuid().ToString();

            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("Core.exerciseB.pdf");

            //using (var httpClient = new HttpClient())
            //{
                //var pdfStream = Task.Run(() => httpClient.GetStreamAsync(url)).Result;
                localPath =
                    Task.Run(() => dependency.SaveFileToDisk(stream, $"{fileName}.pdf")).Result;
            //}

            if (string.IsNullOrWhiteSpace(localPath))
            {
                DisplayAlert("Error baixar PDF", "não foi possivel encontrar o arquivo", "OK");

                return;
            }

            PdfView.Uri = localPath;
        }
    }
}