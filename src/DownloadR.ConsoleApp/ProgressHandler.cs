using System;

using DownloadR.Session;

using Konsole;

namespace DownloadR.ConsoleApp {
    public class ProgressHandler : IDisposable {
        private const int INDENT = -15;

        private readonly DownloadFileOptions _downloadFileOptions;
        private readonly ProgressBar _bar;

        public ProgressHandler(DownloadFileOptions downloadFileOptions, ProgressBar bar) {
            this._downloadFileOptions = downloadFileOptions ?? throw new ArgumentNullException(nameof(downloadFileOptions));
            this._bar = bar ?? throw new ArgumentNullException(nameof(bar));

            this._bar.Refresh(0, $"{"Waiting",INDENT}{this._downloadFileOptions.File}");
        }

        public bool TryUpdateStatus(FileDownloadStatusReport report) {
            if(!this.IsResponsible(report))
                return false;

            Action<FileDownloadStatusReport> action;

            switch(report.Status) {
                case FileDownloadStatusType.Failed:
                    action = updateProgressAsFailed;
                    break;
                case FileDownloadStatusType.Completed:
                    action = updateProgressAsCompleted;
                    break;
                case FileDownloadStatusType.Downloading:
                    action = updateProgress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            action(report);

            return true;
        }


        public bool IsResponsible(FileDownloadStatusReport report) {
            return report.Configuration.Equals(this._downloadFileOptions);
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

        public void Dispose() {
        }

    }
}