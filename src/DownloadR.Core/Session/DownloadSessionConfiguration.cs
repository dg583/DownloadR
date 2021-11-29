using System;

namespace DownloadR.Session {
    /// <summary>
    /// Provides configuration for a download session
    /// </summary>
    public class DownloadSessionConfiguration {
        private int _parallelDownloads;

        /// <summary>
        /// Number of parallel download
        /// </summary>
        public int ParallelDownloads {
            get => this._parallelDownloads;
            set => this._parallelDownloads = value > 0 ? value : 1;
            // Setter available because should be possible to override by args from console
        }

        /// <summary>
        /// The output directory to which the files should be downloaded
        /// </summary>
        public string DownloadDirectory { get; }


        public DownloadSessionConfiguration(string downloadDirectory, int parallelDownloads) {
            downloadDirectory.ThrowIfNotSet(nameof(downloadDirectory));

            this.DownloadDirectory = downloadDirectory;
            this.ParallelDownloads = parallelDownloads;
        }
    }
}
