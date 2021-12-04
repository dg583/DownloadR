using System;
using System.IO;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

using DownloadR.FileParser.Yaml;
using DownloadR.FileParserCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DownloadR.ConsoleApp {
    class Program {
        static void Main(string[] args) {

            var parserResult = args.ParseArgs();

            parserResult.WithParsed(options => {

                using IHost host = CreateHostBuilder(options, args).Build();

                Func<IHost, Options, Task> action = null;

                switch(options.Command?.ToLower()) {
                    case "download":
                        action = handleDownload;
                        break;
                    case "validate":
                        action = handleValidate;
                        break;
                    default:
                        HelpText.AutoBuild(parserResult);
                        break;
                }

                if(action != null)
                    action(host, options).Wait();

            });

            Console.WriteLine("Done - Press any key");
            Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(Options options, string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => {

                services.AddDownloadR(options => {
                    options.UseDownloadManager(provider => {
                        DownloadManagerOptions downloadManagerOptions = new DownloadManagerOptions {
                            WorkingDirectory = _.HostingEnvironment.ContentRootPath
                        };

                        return ActivatorUtilities.CreateInstance<DownloadManager>(provider, downloadManagerOptions);
                    });
                });

                services
                    .AddSingleton<DownloadSessionFileParser>(provider => {
                        var downloadSessionFileReader = ActivatorUtilities.CreateInstance<DownloadSessionFileParser>(provider);

                        //TODO: Readers could be detected by reflection or via plugin 
                        downloadSessionFileReader.AddReader(new YamlDownloadOptionsReader());

                        return downloadSessionFileReader;
                    });

                //TODO: implement log-output to file (use output folder)
                services.AddLogging(builder =>
                        builder
                            .SetMinimumLevel(options.Verbose || options.DryRun ? LogLevel.Trace : LogLevel.Information)
                            .AddConsole());

            });

        static async Task handleDownload(IHost host, Options options) {

            Console.WriteLine("-- DOWNLOAD");

            var downloadManager = host.Services.GetService<IDownloadManager>();
            var downloadSessionFileReader = host.Services.GetService<DownloadSessionFileParser>();

            DownloadSessionLoadResult downloadSessionResult = null;

            using(var fileStream = File.OpenRead(options.SessionFile)) {
                downloadSessionResult = downloadSessionFileReader.LoadDownloadSession(fileStream);
                if(options.ParallelDownload.HasValue)
                    downloadSessionResult.DownloadSession.Settings.ParallelDownloads = options.ParallelDownload.Value;
            }

            using var inter = new ProgressSessionNotificationInterceptor(downloadSessionResult.DownloadSession);

            await downloadManager.StartSessionAsync(downloadSessionResult.DownloadSession, inter);
        }

        static Task handleValidate(IHost host, Options options) {
            return Task.Factory.StartNew(() => Console.WriteLine("-- Validate"));
        }

    }
}
