using System;
using DownloadR.Session;

namespace DownloadR.FileParserCore
{
    /// <summary>
    /// Provides the result of a load process for a <see cref="DownloadSession"/>
    /// </summary>
    public class DownloadSessionLoadResult {
        
        /// <summary>
        /// Contains the <see cref="Exception"/> when <see cref="Succeeded"/> is <c> False</c>
        /// </summary>
        public Exception Exception { get; }
        
        /// <summary>
        /// Contains the <see cref="DownloadSession"/> which was created
        /// </summary>
        public DownloadSession DownloadSession { get; }

        /// <summary>
        /// <c>True</c>, if file was parsed correct, otherwise <c>False</c>
        /// </summary>
        public bool Succeeded => this.DownloadSession != null;

        public DownloadSessionLoadResult(DownloadSession downloadSession) {

            if(downloadSession != null)
                this.DownloadSession = downloadSession;
            else
                this.Exception = new Exception($"{nameof(DownloadSession)} could not be created");
        }

        public DownloadSessionLoadResult(Exception failed) {
            this.Exception = failed;
        }
    }
}