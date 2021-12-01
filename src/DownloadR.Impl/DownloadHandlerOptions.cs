namespace DownloadR {
    public class DownloadHandlerOptions {

        /// <summary>
        /// Gets the working directory
        /// </summary>
        public string WorkingDirectory { get; }

        public DownloadHandlerOptions(string workingDirectory) {
            workingDirectory.ThrowIfNotSet(nameof(workingDirectory));

            this.WorkingDirectory = workingDirectory;
        }
    }
}