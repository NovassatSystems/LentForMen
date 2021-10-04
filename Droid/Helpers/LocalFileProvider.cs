using System;
using System.IO;
using System.Threading.Tasks;
using Droid;
using Core;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalFileProvider))]
namespace Droid
{
    public class LocalFileProvider : ILocalFileProvider
    {

        private readonly string _rootDir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "pdfjs");
        private readonly string _audioReflections = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "audiosReflections");

        public async Task<string> SaveFileToDisk(Stream pdfStream, string fileName)
        {
            if (!Directory.Exists(_rootDir))
                Directory.CreateDirectory(_rootDir);

            var filePath = Path.Combine(_rootDir, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await pdfStream.CopyToAsync(memoryStream);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }

            return filePath;
        }

        public async Task<string> SaveAudioToDisk(Stream pdfStream, string fileName)
        {
            if (!Directory.Exists(_audioReflections))
                Directory.CreateDirectory(_audioReflections);

            var filePath = Path.Combine(_rootDir, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await pdfStream.CopyToAsync(memoryStream);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }

            return filePath;
        }
    }
}