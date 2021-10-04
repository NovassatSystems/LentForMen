using System;
namespace Core
{
    public class MediaInfo
    {
        public byte[] File;
        public TimeSpan Duration;
        public string MimeType;
        public string FileName;
        public string Size;


        public MediaInfo(byte[] file, TimeSpan duration, string mimeType, string fileName, string size = null)
        {
            this.File = file;
            this.Duration = duration;
            this.MimeType = mimeType;
            this.FileName = fileName;
            this.Size = size;
        }
    }
}
