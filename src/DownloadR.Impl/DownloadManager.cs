#nullable enable
using System;
using System.Threading.Tasks;

using DownloadR.Session;

using Microsoft.Extensions.Logging;

namespace DownloadR {
    public class DownloadManager : IDownloadManager {
        private readonly DownloadManagerOptions _options;
        private readonly ISessionHandlerBuilder _sessionHandlerBuilder;
        private readonly ILogger<DownloadManager> _logger;

        public DownloadManager(
            DownloadManagerOptions options,
            ISessionHandlerBuilder sessionHandlerBuilder,
            ILogger<DownloadManager> logger) {
            this._options = options ?? throw new ArgumentNullException(nameof(options));
            this._sessionHandlerBuilder = sessionHandlerBuilder ?? throw new ArgumentNullException(nameof(sessionHandlerBuilder));

            this._logger = logger;
        }

        /// <summary>
        /// Starts a <see cref="DownloadSession"/>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sessionNotificationInterceptor"></param>
        /// <returns></returns>
        public async Task StartSessionAsync(DownloadSession session, ISessionNotificationInterceptor? sessionNotificationInterceptor) {

            using(this._logger.BeginScope("DownloadSession")) {
                var downloadSessionHandler = this.buildDownloadSessionHandler(sessionNotificationInterceptor);
                
                sessionNotificationInterceptor?.StartSession(downloadSessionHandler);
                
                await downloadSessionHandler.StartSessionAsync(session);

                sessionNotificationInterceptor?.SessionCompleted();
            }
        }

        private IDownloadSessionHandler buildDownloadSessionHandler(ISessionNotificationInterceptor? sessionNotificationInterceptor) {
            var downloadSessionHandler = this.getDownloadSessionHandler();

            this.addInterceptions(downloadSessionHandler, sessionNotificationInterceptor);

            this._logger.LogTrace("{0} created: {1}", nameof(IDownloadSessionHandler), downloadSessionHandler.GetType().Name);

            return downloadSessionHandler;
        }

        private void addInterceptions(IDownloadSessionHandler downloadSessionHandler,
            ISessionNotificationInterceptor? interceptor) {
            if(interceptor == null) return;

            downloadSessionHandler.OnDownloadCompleted += (_, report) => { interceptor.OnFileDownloadStatusReport(report); };
            downloadSessionHandler.OnDownloadFailed += (_, report) => { interceptor.OnFileDownloadStatusReport(report); };
            downloadSessionHandler.OnDownloadProgress += (_, report) => { interceptor.OnFileDownloadStatusReport(report); };
            downloadSessionHandler.OnDownloadStarted += (_, report) => { interceptor.OnFileDownloadStatusReport(report); };
        }

        private IDownloadSessionHandler getDownloadSessionHandler() {
            try {
                DownloadHandlerOptions options = new (this._options.WorkingDirectory);

                IDownloadSessionHandler handler = this._sessionHandlerBuilder.BuildDownloadSessionHandler(options);

                return handler;
            }
            catch(Exception e) {
                this._logger.LogCritical(e, e.Message);
                throw new Exception("Error while creating the download session", e);
            }
        }
    }
}
