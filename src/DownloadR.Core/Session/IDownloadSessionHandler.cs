using System;
using System.Threading.Tasks;

namespace DownloadR.Session {
    /// <summary>
    /// Interface for an object that can handle multiple downloads
    /// </summary>
    public interface IDownloadSessionHandler {

        /// <summary>
        /// Triggered, when a download has started
        /// </summary>
        event EventHandler<FileDownloadStatusReport> OnDownloadStarted;

        /// <summary>
        /// Triggered, when a download is complete
        /// </summary>
        event EventHandler<FileDownloadStatusReport> OnDownloadCompleted;

        /// <summary>
        /// Trigger, when a download failed
        /// </summary>
        event EventHandler<FileDownloadStatusReport> OnDownloadFailed;

        /// <summary>
        /// Triggered, when a download notifies a progress update
        /// </summary>
        event EventHandler<FileDownloadStatusReport> OnDownloadProgress;
        

        /// <summary>
        /// Starts a download session
        /// </summary>
        /// <returns></returns>
        Task StartSessionAsync(DownloadSession downloadSession);
    }
}