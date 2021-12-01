using System;
using DownloadR.EventArguments;

namespace DownloadR.Observers {

    /// <summary>
    /// Observer for a single file download
    /// </summary>
    internal class DownloadFileObserver : IObserver<IFileDownloadProgress> {

        /// <summary>
        /// Triggered, when a download is completed
        /// </summary>
        public event EventHandler<DownloadFileNotificationEventArgs> OnDownloadCompleted;

        /// <summary>
        /// Triggered, when a download failed
        /// </summary>
        public event EventHandler<DownloadFileNotificationEventArgs> OnDownloadError;

        /// <summary>
        /// Triggered, when a new progress step is available
        /// </summary>
        public event EventHandler<DownloadFileNotificationEventArgs> OnDownloadProgress;

        private readonly FileDownloadContext _context;

        public DownloadFileObserver(FileDownloadContext context) {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void OnCompleted() {
            this.TriggerOnDownloadCompleted(DownloadFileNotificationEventArgs.BuildForCompleted(this._context));
        }

        public void OnError(Exception error) {
            this.TriggerOnDownloadError(DownloadFileNotificationEventArgs.BuildForException(this._context, error));
        }

        public void OnNext(IFileDownloadProgress value) {
            this.TriggerOnDownloadProgress(DownloadFileNotificationEventArgs.BuildForProgress(this._context, value));
        }

        protected virtual void TriggerOnDownloadCompleted(DownloadFileNotificationEventArgs e) {
            this.OnDownloadCompleted?.Invoke(this, e);
        }

        protected virtual void TriggerOnDownloadError(DownloadFileNotificationEventArgs e) {
            OnDownloadError?.Invoke(this, e);
        }

        protected virtual void TriggerOnDownloadProgress(DownloadFileNotificationEventArgs e) {
            OnDownloadProgress?.Invoke(this, e);
        }
    }
}