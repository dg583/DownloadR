using System;

using DownloadR;
using DownloadR.Session;

using Konsole;

namespace DownloadR.ConsoleApp {
    public class ProgressHandler : IDisposable {
        private const int INDENT = -15;

        private readonly IDownloadSessionHandler _session;
        private readonly DownloadFileOptions _downloadFileOptions;
        private readonly ProgressBar _bar;

        public ProgressHandler(IDownloadSessionHandler session, DownloadFileOptions downloadFileOptions, ProgressBar bar) {
            this._session = session ?? throw new ArgumentNullException(nameof(session));
            this._downloadFileOptions = downloadFileOptions ?? throw new ArgumentNullException(nameof(downloadFileOptions));
            this._bar = bar ?? throw new ArgumentNullException(nameof(bar));

            this._bar.Refresh(0, $"{"Waiting",INDENT}{this._downloadFileOptions.File}");

            this.captureEvents();
        }

        //public async Task Start() {
        //    this.captureEvents();
        //    await this._session.StartAsync();
        //}


        private void captureEvents() {
            this._session.OnDownloadStarted += (sender, report) => { this.doWhenResponsible(report, updateProgressAsStarted); };

            this._session.OnDownloadProgress += (sender, report) => { this.doWhenResponsible(report, updateProgress); };

            this._session.OnDownloadCompleted += (sender, report) => { this.doWhenResponsible(report, updateProgressAsCompleted); };

            this._session.OnDownloadFailed += (sender, report) => { this.doWhenResponsible(report, updateProgressAsFailed); };
        }

        private void doWhenResponsible(FileDownloadStatusReport report, Action<FileDownloadStatusReport> action) {
            if(!report.Configuration.Equals(this._downloadFileOptions)) return;

            action(report);
        }

        private void updateProgress(FileDownloadStatusReport report) {
            this._bar.Refresh(report.Progress.ProgressPercentage, $"{"Download",INDENT}{this._downloadFileOptions.File}");
        }

        private void updateProgressAsCompleted(FileDownloadStatusReport report) {
            this._bar.Refresh(100, $"{"Complete",INDENT}{this._downloadFileOptions.File}");
        }

        private void updateProgressAsFailed(FileDownloadStatusReport report) {
            this._bar.Refresh(0, $"{"FAILED",INDENT}{this._downloadFileOptions.File}");
        }

        private void updateProgressAsStarted(FileDownloadStatusReport report) { }

        public void Dispose() {
        }

    }
}