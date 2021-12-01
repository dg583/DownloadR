#nullable enable
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using DownloadR.Common.Utils;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DownloadR {
    public class DownloadFileTask : Observable<IFileDownloadProgress>, IDownloadFileTask {

        protected readonly DownloadFileConfig DownloadFileConfig;
        private readonly ILogger<DownloadFileTask> _logger;

        private IDownloadFileClient? _downloadFileClient;
        private readonly object _downloadFileClientLock = new();

        /// <summary>
        /// Gets the instance of the <see cref="DownloadFileClient"/>
        /// </summary>
        protected IDownloadFileClient DownloadFileClient {
            get {
                if(this._downloadFileClient == null) {
                    lock(this._downloadFileClientLock) {
                        if(this._downloadFileClient == null) {
                            this._downloadFileClient = this.buildDownloadFileClient();
                            this._downloadFileClient.DownloadFileCompleted += this.handleOnDownloadFileCompleted;
                            this._downloadFileClient.DownloadProgressChanged += this.handleOnDownloadProgressChanged;
                        }
                    }
                }
                return this._downloadFileClient;
            }
        }

        #region ctor

        public DownloadFileTask(DownloadFileConfig config, ILogger<DownloadFileTask>? logger) {
            config.ThrowIfNotSet(nameof(config));

            this.DownloadFileConfig = config;
            this._logger = logger ?? NullLoggerFactory.Instance.CreateLogger<DownloadFileTask>();
        }

        #endregion

        /// <summary>
        /// Get the size of the file to download
        /// </summary>
        /// <returns></returns>
        public long GetFileSize() => this.getFileSize();

        /// <summary>
        /// Starts the download process
        /// </summary>
        /// <returns></returns>
        public async Task StartDownloadAsync() {
            await Task.Factory.StartNew(this.startDownloadFile);
        }

        private void startDownloadFile() {
            try {
                this.logStart();

                //Check if file exists -> if yes, stop download and send notification to observers
                if(File.Exists(this.DownloadFileConfig.WriteToFile))
                    this.handleOnDownloadFileCompleted(this, new AsyncCompletedEventArgs(new Exception($"The file '{this.DownloadFileConfig.WriteToFile}' already exists"), true, null));
                else
                    this.DownloadFileClient.StartDownload();
            }
            catch(Exception e) {
                this._logger.LogError(e, $"Could not start download from '{this.DownloadFileConfig.Uri}': {e.Message}");
                this.SendErrorToObservers(e);
            }
        }

        private void handleOnDownloadFileCompleted(object? sender, AsyncCompletedEventArgs e) {
            this.logOnDownloadCompleted(e);

            if(e.Error != null) {
                base.SendErrorToObservers(e.Error);
            }
            else {
                base.SendCompletedToObservers();
            }
        }

        private void handleOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            this.logOnProgressChanged(e);

            base.SendDataToObservers(new FileDownloadProgress(e.ProgressPercentage, e.TotalBytesToReceive, e.BytesReceived));
        }

        private long getFileSize() {
            if(this.DownloadFileClient.TryGetFileSize(out long fileSize, out Exception? e)) {
                return fileSize;
            }

            throw e ?? throw new Exception("Something went wrong- but I don't know what :-/");
            //TODO: Throw useful exception
        }

        private IDownloadFileClient buildDownloadFileClient() {
            try {
                IDownloadFileClient downloadFileClient = this.BuildClient();

                this._logger.LogTrace($"{nameof(IDownloadFileClient)} created, using {downloadFileClient.GetType().FullName}");

                return downloadFileClient;
            }
            catch(Exception e) {
                this._logger.LogCritical(e, $"Could not retrieve {nameof(IDownloadFileClient)}");
                throw new ApplicationException($"Error while resolving {nameof(IDownloadFileClient)}", e);
            }
        }

        protected virtual IDownloadFileClient BuildClient() {
            //Protected virtual -> Seam for testing
            IDownloadFileClient client = new WebClientDownloadFileClient(this.DownloadFileConfig);
            return client;
        }

        #region logging

        private void logStart() {
            if(this._logger.IsEnabled(LogLevel.Trace))
                this._logger.LogTrace($"Download '{this.DownloadFileConfig.Uri}' started");
        }

        private void logOnProgressChanged(DownloadProgressChangedEventArgs eventArgs) {
            if(this._logger.IsEnabled(LogLevel.Trace))
                this._logger.LogTrace($"{eventArgs.BytesReceived}/{eventArgs.TotalBytesToReceive}");
        }

        private void logOnDownloadCompleted(AsyncCompletedEventArgs eventArgs) {
            if(eventArgs.Error != null) {
                this._logger.LogError(666, eventArgs.Error, $"Download failed: {eventArgs.Error.Message}");
            }
            else {
                this._logger.LogInformation($"Download '{this.DownloadFileConfig.Uri}' complete");
            }
        }

        #endregion
    }
}
