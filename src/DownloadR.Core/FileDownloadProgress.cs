namespace DownloadR
{
    public class FileDownloadProgress : IFileDownloadProgress {
        
        /// <summary>
        /// Total percentage of download
        /// </summary>
        public int ProgressPercentage { get; }
        
        /// <summary>
        /// Total bytes of the file to download
        /// </summary>
        public long TotalBytes { get; }

        /// <summary>
        /// Received bytes of the file
        /// </summary>
        public long BytesReceived { get; }

        public FileDownloadProgress(int progressPercentage, long totalBytes, long bytesReceived) {
            this.ProgressPercentage = progressPercentage > 0 ? progressPercentage : 0;
            this.TotalBytes = totalBytes > 0 ? totalBytes : 0;
            this.BytesReceived = bytesReceived > 0 ? bytesReceived : 0;
        }
    }
}