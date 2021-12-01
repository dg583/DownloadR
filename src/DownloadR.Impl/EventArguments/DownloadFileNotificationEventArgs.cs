using System;
using System.ComponentModel;

namespace DownloadR.EventArguments {

    /// <summary>
    /// Defines an event that was created for a download
    /// </summary>
    public class DownloadFileNotificationEventArgs : EventArgs {

        /// <summary>
        /// <see cref="FileDownloadProgress"/>
        /// </summary>
        public IFileDownloadProgress Progress { get; internal set; }

        /// <summary>
        /// Defines the status of progress
        /// </summary>
        public FileDownloadStatusType Status { get; }

        /// <summary>
        /// Provides the <see cref="FileDownloadContext"/>
        /// </summary>
        public FileDownloadContext Context { get; }


        /// <summary>
        /// Occurred <see cref="Exception"/> if <see cref="Status"/> is <see cref="FileDownloadStatusType.Downloading"/>
        /// </summary>
        public Exception Exception { get; internal set; }


        internal DownloadFileNotificationEventArgs(FileDownloadStatusType status, FileDownloadContext context) {
            if(!Enum.IsDefined(typeof(FileDownloadStatusType), status))
                throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(FileDownloadStatusType));

            context.ThrowIfNotSet(nameof(context));

            this.Status = status;
            this.Context = context;
        }

        public static DownloadFileNotificationEventArgs BuildForProgress(FileDownloadContext context, IFileDownloadProgress progress) {
            return new DownloadFileNotificationEventArgs(FileDownloadStatusType.Downloading, context) {
                Progress = progress ?? new FileDownloadProgress(0, 0, 0) // Exception 
            };
        }

        public static DownloadFileNotificationEventArgs BuildForException(FileDownloadContext context, Exception exception) {
            return new DownloadFileNotificationEventArgs(FileDownloadStatusType.Failed, context) {
                Exception = exception,
                Progress = new FileDownloadProgress(0, 0, 0)
            };
        }

        public static DownloadFileNotificationEventArgs BuildForCompleted(FileDownloadContext context) {
            long fileSize = context.DownloadFileTask.GetFileSize();

            return new DownloadFileNotificationEventArgs(FileDownloadStatusType.Completed, context) {
                Progress = new FileDownloadProgress(100, fileSize, fileSize)
            };
        }
    }

}
