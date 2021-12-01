using System;
using System.ComponentModel;
using System.Net;

namespace DownloadR
{
    public interface IDownloadFileClient {
        /// <summary>
        /// Triggers, when the download progress changed
        /// </summary>
        event DownloadProgressChangedEventHandler DownloadProgressChanged;

        /// <summary>
        /// Triggers, when the download is completed
        /// </summary>
        event AsyncCompletedEventHandler DownloadFileCompleted;

        /// <summary>
        /// Starts the download
        /// </summary>
        void StartDownload();

        /// <summary>
        /// Cancels a download
        /// </summary>
        void CancelDownload();

        /// <summary>
        /// Tries to determine the size of the file to download
        /// </summary>
        /// <param name="fileSize">The size of the file</param>
        /// <param name="failedException">Exception, that contains the failed reason if the result ist <c>False</c></param>
        /// <returns><c>True</c>, if the file exists and a size could be retrieved; Otherwise <c>False</c></returns>
        bool TryGetFileSize(out long fileSize, out Exception? failedException);
    }
}