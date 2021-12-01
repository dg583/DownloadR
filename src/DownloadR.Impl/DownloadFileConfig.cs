namespace DownloadR
{
    public class DownloadFileConfig {
        /// <summary>
        /// Path to file where the download should be stored
        /// </summary>
        public string WriteToFile { get; }

        /// <summary>
        /// URI to the file to download
        /// </summary>
        public string Uri { get; }

        public DownloadFileConfig(string writeToFile, string uri) {
            uri.ThrowIfNotSet(nameof(uri));
            writeToFile.ThrowIfNotSet(nameof(writeToFile));

            this.WriteToFile = writeToFile;
            this.Uri = uri;
        }
    }
}