namespace DownloadR {
    
    /// <summary>
    /// Defines the available states of a file download
    /// </summary>
    public enum FileDownloadStatusType {
        /// <summary>
        /// An error occurred while downloading the file
        /// </summary>
        Failed,
        /// <summary>
        /// The download is complete
        /// </summary>
        Completed,
        /// <summary>
        /// A new status update 
        /// </summary>
        Downloading
    }
}