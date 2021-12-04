using System;

using DownloadR.Session;

using Microsoft.Extensions.DependencyInjection;

namespace DownloadR.Factories {
    /// <summary>
    /// The default implementation of the <see cref="ISessionHandlerFactory"/>
    /// </summary>
    public class DefaultSessionHandlerFactory : ISessionHandlerFactory {
        private readonly IServiceProvider _serviceProvider;

        public DefaultSessionHandlerFactory(IServiceProvider serviceProvider) {

            serviceProvider.ThrowIfNotSet(nameof(serviceProvider));
            this._serviceProvider = serviceProvider;
        }

        public IDownloadSessionHandler CreateDownloadSessionHandler(DownloadHandlerOptions options) {


            return ActivatorUtilities.CreateInstance<DownloadSessionHandler>(
                this._serviceProvider,
                options);
        }
    }
}