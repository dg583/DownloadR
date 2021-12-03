using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

using DownloadR.FileParser.Yaml;
using DownloadR.FileParserCore;
using DownloadR.Session;

using Konsole;

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

                        return ActivatorUtilities.CreateInstance<DownloadR.DownloadManager>(provider, downloadManagerOptions);
                    });

                    options.UseTaskBuilder(provider => {
                        ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                        //TODO: If dryrun -> Inject DryRunFileTaskBuilder instead
                        return new DefaultDownloadTaskBuilder(loggerFactory);
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

            var inter = new Interceptor(downloadSessionResult.DownloadSession);

            var dm = downloadManager.StartSessionAsync(downloadSessionResult.DownloadSession, inter);

            await dm;

        }

        static Task handleValidate(IHost host, Options options) {
            return Task.Factory.StartNew(() => Console.WriteLine("-- Validate"));
        }

    }

    public class Interceptor : ISessionNotificationInterceptor, IDisposable {
        private readonly DownloadSession _downloadSession;

        private readonly List<ProgressHandler> _progressHandlers = new List<ProgressHandler>();

        public Interceptor(DownloadSession downloadSession) {
            downloadSession.ThrowIfNotSet(nameof(downloadSession));

            this._downloadSession = downloadSession;
        }

        public void StartSession(IDownloadSessionHandler downloadSessionHandler) {

            foreach(DownloadFileOptions downloadFileOptions in this._downloadSession.Configuration) {
                ProgressHandler progressHandler = new ProgressHandler(
                    downloadSessionHandler,
                    downloadFileOptions,
                    new ProgressBar(100)
                );

                this._progressHandlers.Add(progressHandler);
            }
        }

        public void SessionCompleted()
        {
            
        }

        public void OnFileDownloadStatusReport(FileDownloadStatusReport fileDownloadStatusReport) {
            
        }

        public void Dispose() {
            this._progressHandlers?.ForEach(x => x.Dispose());
        }
    }
}
