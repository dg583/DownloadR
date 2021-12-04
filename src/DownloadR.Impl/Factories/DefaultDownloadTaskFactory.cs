using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DownloadR.Factories {
    /// <summary>
    /// The default implementation of the <see cref="IDownloadTaskFactory"/>
    /// </summary>
    public class DefaultDownloadTaskFactory : IDownloadTaskFactory {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DefaultDownloadTaskFactory> _logger;

        public DefaultDownloadTaskFactory(IServiceProvider serviceProvider, ILogger<DefaultDownloadTaskFactory> logger) {
            serviceProvider.ThrowIfNotSet(nameof(serviceProvider));
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }

        public IDownloadFileTask CreateDownloadFileTask(DownloadFileConfig downloadFileConfig) {
            using var scope = this._logger.BeginScope();
            return ActivatorUtilities.CreateInstance<DownloadFileTask>(this._serviceProvider, downloadFileConfig);
        }
    }
}