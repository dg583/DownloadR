#nullable enable
using System.Threading.Tasks;

using DownloadR.Session;

namespace DownloadR {
    public interface IDownloadManager {
        /// <summary>
        /// Starts a download session
        /// </summary>
        /// <param name="session"><see cref="DownloadSession"/></param>
        /// <param name="interceptor"><see cref="ISessionNotificationInterceptor"/></param>
        /// <returns></returns>
        Task StartSessionAsync(DownloadSession session, ISessionNotificationInterceptor? interceptor);
    }
}
