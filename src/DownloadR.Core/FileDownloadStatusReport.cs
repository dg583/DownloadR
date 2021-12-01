using System;
using System.ComponentModel;

using DownloadR.Session;

namespace DownloadR {
    
    /// <summary>
    /// Provides information about a status update for a specific download
    /// </summary>
    public class FileDownloadStatusReport {
        /// <summary>
        /// Get the <see cref="DownloadFileOptions"/> of the assigned download
        /// </summary>
        public DownloadFileOptions Configuration { get; }

        /// <summary>
        /// Get the process of the download
        /// </summary>
        public IFileDownloadProgress Progress { get; }

        /// <summary>
        /// Gets the Status of the file download
        /// </summary>
        public FileDownloadStatusType Status { get; }


        /// <summary>
        /// Occurred <see cref="Exception"/> if <see cref="Status"/> is <see cref="FileDownloadStatusType.Downloading"/>
        /// </summary>
        public Exception Exception { get; }


        public FileDownloadStatusReport(
            DownloadFileOptions configuration,
            IFileDownloadProgress progress,
            FileDownloadStatusType status) {

            if(!Enum.IsDefined(typeof(FileDownloadStatusType), status))
                throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(FileDownloadStatusType));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Progress = progress ?? throw new ArgumentNullException(nameof(progress));
            this.Status = status;
        }

        public FileDownloadStatusReport(
            DownloadFileOptions configuration,
            IFileDownloadProgress progress,
            Exception ex) : this(configuration, progress, FileDownloadStatusType.Failed) {
            this.Exception = ex;
        }
    }
}