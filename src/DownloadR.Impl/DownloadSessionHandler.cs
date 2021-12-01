using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DownloadR.EventArguments;
using DownloadR.Observers;
using DownloadR.Session;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DownloadR {
    /// <summary>
    /// Handles a download session
    /// </summary>
    public class DownloadSessionHandler : IDownloadSessionHandler {
        //TODO: Could also be an observable instead of triggering events

        #region events

        public event EventHandler<FileDownloadStatusReport> OnDownloadStarted;
        public event EventHandler<FileDownloadStatusReport> OnDownloadCompleted;
        public event EventHandler<FileDownloadStatusReport> OnDownloadFailed;
        public event EventHandler<FileDownloadStatusReport> OnDownloadProgress;

        #endregion

        private readonly DownloadHandlerOptions _options;
        private readonly IDownloadTaskBuilder _downloadTaskBuilder;
        private readonly ILogger<DownloadSessionHandler> _logger;

        public DownloadSessionHandler(DownloadHandlerOptions options, IDownloadTaskBuilder downloadTaskBuilder, ILogger<DownloadSessionHandler> logger) {
            this._options = options ?? throw new ArgumentNullException(nameof(options));
            this._downloadTaskBuilder = downloadTaskBuilder ?? throw new ArgumentNullException(nameof(downloadTaskBuilder));
            this._logger = logger ?? NullLogger<DownloadSessionHandler>.Instance;
        }

        public Task StartSessionAsync(DownloadSession downloadSession) {
            try {
                return Task.Factory.StartNew(() => this.start(downloadSession));
            }
            catch(Exception e) {
                //TODO: Throw readable ex
                throw;
            }
        }

        private void start(DownloadSession downloadSession) {
            //TODO: Throws exception if file already exists - pass ex to callee
            var contexts = this.createFileDownloadContextsFromDownloadSession(downloadSession);

            Parallel.ForEach(contexts,
                new ParallelOptions {
                    MaxDegreeOfParallelism = downloadSession.Settings.ParallelDownloads
                },
                (fileDownloadContext) => {
                    this.logFileDownloadContextCreated(fileDownloadContext);

                    this.executeDownloadFileTask(fileDownloadContext);
                });
        }

        private void executeDownloadFileTask(FileDownloadContext fileDownloadContext) {
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, 1);

            var observer = new DownloadFileObserver(fileDownloadContext);

            observer.OnDownloadCompleted += handleOnDownloadCompleted;
            observer.OnDownloadError += handleOnDownloadError;
            observer.OnDownloadProgress += handleOnDownloadProgress;

            using(fileDownloadContext.DownloadFileTask.Subscribe(observer)) {
                
                //Semaphore to enable usage of parallel.foreach - must wait until download is complete
                using(fileDownloadContext.DownloadFileTask.Subscribe(
                    new ReleaseSemaphoreObserver(semaphoreSlim))) {
                    Task t = fileDownloadContext.DownloadFileTask.StartDownloadAsync();
                    t.Wait();
                    semaphoreSlim.Wait();
                }
            }
        }

        private IEnumerable<FileDownloadContext> createFileDownloadContextsFromDownloadSession(DownloadSession downloadSession) {
            FileDownloadContextBuilder builder =
                new FileDownloadContextBuilder(this._options, this._downloadTaskBuilder);

            return builder.Build(downloadSession);
        }

        #region event-handling

        private void handleOnDownloadProgress(object? sender, DownloadFileNotificationEventArgs e) {
            this.OnDownloadProgress?.Invoke(this, e.ToFileDownloadStatusReport());
        }

        private void handleOnDownloadError(object? sender, DownloadFileNotificationEventArgs e) {
            this.OnDownloadFailed?.Invoke(this, e.ToFileDownloadStatusReport());
        }

        private void handleOnDownloadCompleted(object? sender, DownloadFileNotificationEventArgs e) {
            this.OnDownloadCompleted?.Invoke(this, e.ToFileDownloadStatusReport());
        }

        #endregion

        #region logging

        private void logFileDownloadContextCreated(FileDownloadContext ctx) {
            using(this._logger.BeginScope($"{nameof(FileDownloadContext)}", ("DownloadUri", ctx.Config.Uri))) {
                this._logger.LogTrace($"Write to file: {ctx.Config.WriteToFile}");
            }

        }

        #endregion
    }
}
