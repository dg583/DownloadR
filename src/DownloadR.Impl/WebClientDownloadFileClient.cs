using System;
using System.ComponentModel;
using System.Net;

namespace DownloadR
{
    /// <summary>
    /// Client to download a file from a specific <see cref="Uri"/> to a file
    /// </summary>
    public class WebClientDownloadFileClient : IDownloadFileClient {
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;
        public event AsyncCompletedEventHandler? DownloadFileCompleted;

        private WebClient _webClient;

        private readonly DownloadFileConfig _downloadFileConfig;

        public WebClientDownloadFileClient(DownloadFileConfig downloadFileConfig) {
            downloadFileConfig.ThrowIfNotSet(nameof(downloadFileConfig));

            this._downloadFileConfig = downloadFileConfig;
        }

        /// <summary>
        /// Starts the download
        /// </summary>
        public void StartDownload() {
            this.startDownloadFile();
        }

        public void CancelDownload() {
            this._webClient?.CancelAsync();
            this._webClient?.Dispose();
            this._webClient = null;

            this.TriggerDownloadFileCompleted(new AsyncCompletedEventArgs(null, true, null));
        }

        /// <summary>
        /// Tries to determine the size of the file to download
        /// </summary>
        /// <param name="fileSize">The size of the file</param>
        /// <param name="failedException">Exception, that contains the failed reason if the result ist <c>False</c></param>
        /// <returns><c>True</c>, if the file exists and a size could be retrieved; Otherwise <c>False</c></returns>
        public bool TryGetFileSize(out long fileSize, out Exception? failedException) {
            return this.tryGetFileSizeFromUri(out fileSize, out failedException);
        }

        private void startDownloadFile() {
            this._webClient = new();

            this._webClient.DownloadProgressChanged += (_, args) => this.TriggerDownloadProgressChanged(args);
            this._webClient.DownloadFileCompleted += (_, args) => this.TriggerDownloadFileCompleted(args);

            this._webClient.DownloadFileAsync(new Uri(this._downloadFileConfig.Uri), this._downloadFileConfig.WriteToFile);


        }

        /// <summary>
        /// Tries to determine the size of the file to download
        /// </summary>
        /// <param name="fileSize">The size of the file</param>
        /// <param name="failedException">Exception, that contains the failed reason if the result ist <c>False</c></param>
        /// <returns><c>True</c>, if the file exists and a size could be retrieved; Otherwise <c>False</c></returns>
        private bool tryGetFileSizeFromUri(out long fileSize, out Exception? failedException) {
            failedException = null;

            try {
                using(WebClient client = new WebClient()) {
                    using(var _ = client.OpenRead(new Uri(this._downloadFileConfig.Uri))) {

                        fileSize = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                        return true;
                    }
                }
            }
            catch(Exception? e) {
                failedException = e;
            }

            fileSize = -1;
            return false;
        }


        protected void TriggerDownloadProgressChanged(DownloadProgressChangedEventArgs e) {
            DownloadProgressChanged?.Invoke(this, e);
        }

        protected void TriggerDownloadFileCompleted(AsyncCompletedEventArgs e) {
            DownloadFileCompleted?.Invoke(this, e);
        }
    }
}