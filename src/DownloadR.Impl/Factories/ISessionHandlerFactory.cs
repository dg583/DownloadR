using DownloadR.Session;

namespace DownloadR.Factories
{
    /// <summary>
    /// Provides methods to create <see cref="IDownloadSessionHandler"/>
    /// </summary>
    public interface ISessionHandlerFactory {
        //Interface for testing-seam, DI, Setup

        /// <summary>
        /// Creates an <see cref="IDownloadSessionHandler"/> for the <param name="options"></param>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IDownloadSessionHandler CreateDownloadSessionHandler(DownloadHandlerOptions options);
    }
}