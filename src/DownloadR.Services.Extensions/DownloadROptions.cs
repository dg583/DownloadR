using System;
using DownloadR.Factories;
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

        public void UseTaskBuilder(Func<IServiceProvider, IDownloadTaskFactory> factoryMethod,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient) {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(
                typeof(IDownloadTaskFactory), factoryMethod, serviceLifetime);

            this.IDownloadTaskBuilderServiceDescriptor = serviceDescriptor;
        }

        public DownloadROptions() {
            this.IDownloadTaskBuilderServiceDescriptor = ServiceDescriptor.Scoped<IDownloadTaskFactory, DefaultDownloadTaskFactory>();
            this.ISessionHandlerBuilderServiceDescriptor = ServiceDescriptor.Scoped<ISessionHandlerFactory, DefaultSessionHandlerFactory>();
        }
    }
}
