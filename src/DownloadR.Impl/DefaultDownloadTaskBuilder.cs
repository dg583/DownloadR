using Microsoft.Extensions.Logging;

namespace DownloadR {
    /// <summary>
    /// The default implementation of the <see cref="IDownloadTaskBuilder"/>
    /// </summary>
    public class DefaultDownloadTaskBuilder : IDownloadTaskBuilder {
        private readonly ILoggerFactory _loggerFactory;

        public DefaultDownloadTaskBuilder(ILoggerFactory loggerFactory) {
            this._loggerFactory = loggerFactory;
        }

        public IDownloadFileTask Build(DownloadFileConfig downloadFileConfig) {
            return new DownloadFileTask(downloadFileConfig, this._loggerFactory.CreateLogger<DownloadFileTask>());
        }
    }
}