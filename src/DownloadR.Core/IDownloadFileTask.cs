using System;
using System.Threading.Tasks;

namespace DownloadR {
    /// <summary>
    /// Interface for an object that handles a download
    /// </summary>
    public interface IDownloadFileTask : IObservable<IFileDownloadProgress> {
        
        /// <summary>
        /// Get the size of the file to download
        /// </summary>
        /// <returns></returns>
        long GetFileSize();


        /// <summary>
        /// Starts the download for a file
        /// </summary>
        public Task StartDownloadAsync();
    }
}