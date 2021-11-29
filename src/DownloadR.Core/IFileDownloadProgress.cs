namespace DownloadR {
    /// <summary>
    /// Defines the progress of a file download
    /// </summary>
    public interface IFileDownloadProgress {
        /// <summary>
        /// The percentage value of the download progress
        /// </summary>
        int ProgressPercentage { get; }

        /// <summary>
        /// The total size of the file
        /// </summary>
        long TotalBytes { get; }

        /// <summary>
        /// The already received bytes
        /// </summary>
        long BytesReceived { get; }
    }
}