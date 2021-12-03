using System;
using DownloadR.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DownloadR
{
    /// <summary>
    /// The default implementation of the <see cref="ISessionHandlerBuilder"/>
    /// </summary>
    public class DefaultSessionHandlerBuilder : ISessionHandlerBuilder {
        private readonly IDownloadTaskBuilder _downloadTaskBuilder;
        private readonly ILoggerFactory _loggerFactory;

        public DefaultSessionHandlerBuilder(IDownloadTaskBuilder downloadTaskBuilder, ILoggerFactory loggerFactory) {
            this._downloadTaskBuilder = downloadTaskBuilder ?? throw new ArgumentNullException(nameof(downloadTaskBuilder));
            this._loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
        }

        public IDownloadSessionHandler BuildDownloadSessionHandler(DownloadHandlerOptions options) {
            DownloadSessionHandler handler =
                new DownloadSessionHandler(options, this._downloadTaskBuilder, this._loggerFactory.CreateLogger<DownloadSessionHandler>());
            return handler;
        }
    }
}