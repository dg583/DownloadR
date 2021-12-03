using DownloadR.Session;

namespace DownloadR
{
    /// <summary>
    /// Provides methods to create <see cref="IDownloadSessionHandler"/>
    /// </summary>
    public interface ISessionHandlerBuilder {
        //Interface for testing-seam, DI, Setup

        /// <summary>
        /// Builds a <see cref="IDownloadSessionHandler"/> for the <param name="options"></param>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IDownloadSessionHandler BuildDownloadSessionHandler(DownloadHandlerOptions options);
    }
}