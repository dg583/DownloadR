using System;
using System.Collections.Generic;

namespace DownloadR.Session {
    
    /// <summary>
    /// Defines a download session with options and files to download
    /// </summary>
    public class DownloadSession {

        /// <summary>
        /// <see cref="DownloadSessionConfiguration"/>
        /// </summary>
        public DownloadSessionConfiguration Settings { get; }

        /// <summary>
        /// Collection of <see cref="DownloadFileOptions"/>
        /// </summary>
        public IReadOnlyCollection<DownloadFileOptions> Configuration { get; }

        public DownloadSession(DownloadSessionConfiguration settings, IEnumerable<DownloadFileOptions> configurations) {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            //TODO: Check for doubles
            this.Configuration = new List<DownloadFileOptions>(configurations);
        }
    }
}