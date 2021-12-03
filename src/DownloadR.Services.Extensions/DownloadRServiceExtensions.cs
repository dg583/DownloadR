using System;
using Microsoft.Extensions.DependencyInjection;

namespace DownloadR
{
    public static class DownloadRServiceExtensions {

        public static void AddDownloadR(this IServiceCollection services, Action<DownloadROptions> optionsResolver) {
            DownloadROptions options = new DownloadROptions();

            optionsResolver.Invoke(options);

            services.Add(options.IDownloadManagerServiceDescriptor);
            services.Add(options.IDownloadTaskBuilderServiceDescriptor);
            services.Add(options.ISessionHandlerBuilderServiceDescriptor);
        }

    }
}