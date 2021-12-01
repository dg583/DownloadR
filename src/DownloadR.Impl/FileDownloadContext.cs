using System;
using DownloadR.Session;

namespace DownloadR
{
    public class FileDownloadContext {
        public IDownloadFileTask DownloadFileTask { get; }

        public DownloadFileOptions DownloadConfiguration { get; }
        public DownloadFileConfig Config { get; }


        public FileDownloadContext(
            DownloadFileOptions downloadConfiguration,
            DownloadFileConfig config, IDownloadFileTask downloadFileTask) {

            this.DownloadConfiguration = downloadConfiguration ?? throw new ArgumentNullException(nameof(downloadConfiguration));
            this.Config = config ?? throw new ArgumentNullException(nameof(config));
            this.DownloadFileTask = downloadFileTask ?? throw new ArgumentNullException(nameof(downloadFileTask));
        }
    }
}