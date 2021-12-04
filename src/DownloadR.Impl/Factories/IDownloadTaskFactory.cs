namespace DownloadR.Factories {

    /// <summary>
    /// Defines a builder for <see cref="IDownloadFileTask"/>
    /// </summary>
    /// <remarks>
    ///Seam for testing
    /// </remarks>
    public interface IDownloadTaskFactory {

        /// <summary>
        /// Creates an <see cref="IDownloadFileTask"/> for the <param name="downloadFileConfig"></param>
        /// </summary>
        /// <param name="downloadFileConfig"><see cref="DownloadFileConfig"/></param>
        /// <returns></returns>
        IDownloadFileTask CreateDownloadFileTask(DownloadFileConfig downloadFileConfig);
    }
}
