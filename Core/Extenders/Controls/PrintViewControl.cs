using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core
{
    public class PrintViewControl : ContentView
    {
        public Action ExecutePrint;
        public Action<byte[]> OnPrintCompleted;

        public string FileName { get; set; }

        public PrintViewControl()
        {
            OnPrintCompleted = (byteArray) => {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var tmp = Path.Combine(documents, "tmp");
                Directory.CreateDirectory(tmp);
                var path = Path.Combine(tmp, $"{FileName}.png");
                if (File.Exists(path))
                    File.Delete(path);
                File.WriteAllBytes(path, byteArray);
                Share.RequestAsync(new ShareFileRequest
                {
                    Title = FileName,
                    File = new ShareFile(path)
                });
            };
        }
    }
}