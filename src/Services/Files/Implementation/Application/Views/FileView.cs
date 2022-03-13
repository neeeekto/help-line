using System;

namespace HelpLine.Services.Files.Application.Views
{
    public class FileView
    {
        public string FileId { get; internal set; }
        public string? Name { get; internal set; }
        public Uri DownloadUri { get; internal set; }
        public Uri? PreviewUri { get; internal set; }
        public long Size { get; internal set; }
        public string Type { get; internal set; }
        public string? Rotate { get; internal set; }
        public DateTime LastModified { get; internal set; }

    }
}
