using System;
using System.Collections.Generic;
using System.Linq;
using DownloadR.Session;
using Konsole;

namespace DownloadR.ConsoleApp
{
    internal class ProgressSessionNotificationInterceptor : ISessionNotificationInterceptor, IDisposable {
        private readonly DownloadSession _downloadSession;

        private readonly List<ProgressHandler> _progressHandlers = new List<ProgressHandler>();

        public ProgressSessionNotificationInterceptor(DownloadSession downloadSession) {
            downloadSession.ThrowIfNotSet(nameof(downloadSession));

            this._downloadSession = downloadSession;
            this.initProgressHandlers();
        }

        private void initProgressHandlers() {
            foreach(DownloadFileOptions downloadFileOptions in this._downloadSession.Configuration) {
                ProgressHandler progressHandler = new ProgressHandler(
                    downloadFileOptions,
                    new ProgressBar(100)
                );

                this._progressHandlers.Add(progressHandler);
            }
        }

        public void SessionCompleted() {

        }

        public void OnFileDownloadStatusReport(FileDownloadStatusReport fileDownloadStatusReport) {
            foreach(ProgressHandler handler in this._progressHandlers.Where(x => x.IsResponsible(fileDownloadStatusReport))) {
                handler.TryUpdateStatus(fileDownloadStatusReport);
            }
        }

        public void Dispose() {
            this._progressHandlers?.ForEach(x => x.Dispose());
        }
    }
}