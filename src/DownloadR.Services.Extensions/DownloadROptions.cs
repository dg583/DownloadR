using System;
using Microsoft.Extensions.DependencyInjection;

namespace DownloadR {

    public class DownloadROptions {

        internal ServiceDescriptor IDownloadManagerServiceDescriptor { get; private set; }
        internal ServiceDescriptor IDownloadTaskBuilderServiceDescriptor { get; private set; }

        internal ServiceDescriptor ISessionHandlerBuilderServiceDescriptor { get; private set; }


        public void UseDownloadManager(Func<IServiceProvider, IDownloadManager> factoryMethod, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) {

            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(
                typeof(IDownloadManager), factoryMethod, serviceLifetime);

            this.IDownloadManagerServiceDescriptor = serviceDescriptor;
        }

        public void UseTaskBuilder(Func<IServiceProvider, IDownloadTaskBuilder> factoryMethod,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient) {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(
                typeof(IDownloadTaskBuilder), factoryMethod, serviceLifetime);

            this.IDownloadTaskBuilderServiceDescriptor = serviceDescriptor;
        }

        public DownloadROptions() {
            this.IDownloadTaskBuilderServiceDescriptor = ServiceDescriptor.Scoped<IDownloadTaskBuilder, DefaultDownloadTaskBuilder>();
            this.ISessionHandlerBuilderServiceDescriptor = ServiceDescriptor.Scoped<ISessionHandlerBuilder, DefaultSessionHandlerBuilder>();
        }
    }
}
