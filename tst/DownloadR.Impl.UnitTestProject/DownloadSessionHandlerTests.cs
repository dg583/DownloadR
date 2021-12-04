using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DownloadR.Factories;
using DownloadR.Impl.UnitTestProject.Fakes;
using DownloadR.Session;

using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using NUnit.Framework;

namespace DownloadR.Impl.UnitTestProject {
    public class DownloadSessionHandlerTests {

        [Test]
        public void CreateInstance() {

            Mock<IDownloadTaskFactory> downloadTaskBuilder = new Mock<IDownloadTaskFactory>();
            downloadTaskBuilder.Setup(x => x.CreateDownloadFileTask(It.IsAny<DownloadFileConfig>()))
                .Returns(() => new DownloadFileTaskStub());


            string workingDir = TestContext.CurrentContext.WorkDirectory;

            DownloadSessionConfiguration downloadSessionConfiguration = new DownloadSessionConfiguration("_out", 1);

            DownloadFileOptions[] fileDownloadOptions = Array.Empty<DownloadFileOptions>();

            var downloadSession = new DownloadSession(downloadSessionConfiguration, fileDownloadOptions);

            var options = new DownloadHandlerOptions(workingDir);
            var handler = new DownloadSessionHandler(options, downloadTaskBuilder.Object, new NullLogger<DownloadSessionHandler>());

            Assert.IsNotNull(handler);
        }

        [Test]
        public async Task Verify_Tasks_Are_executed_parallel() {

            Mock<IDownloadTaskFactory> downloadTaskBuilder = new Mock<IDownloadTaskFactory>();
            downloadTaskBuilder.Setup(x => x.CreateDownloadFileTask(It.IsAny<DownloadFileConfig>()))
                .Returns(() => new DownloadFileTaskStub());


            string workingDir = TestContext.CurrentContext.WorkDirectory;

            DownloadSessionConfiguration downloadSessionConfiguration = new DownloadSessionConfiguration("_out", 2);

            DownloadFileOptions[] fileDownloadOptions = new[]
            {
                new DownloadFileOptions
                {
                    File = "test1.dat", Overwrite = true, Sha256 = "08154711", Url = "https://download.dev/file.dat"
                },
                new DownloadFileOptions
                {
                    File = "test2.dat", Overwrite = true, Sha256 = "08154711", Url = "https://download.dev/file.dat"
                }
            };

            var downloadSession = new DownloadSession(downloadSessionConfiguration, fileDownloadOptions);

            var options = new DownloadHandlerOptions(workingDir);

            var handler = new DownloadSessionHandlerMock(
                options, 
                downloadTaskBuilder.Object, 
                new NullLogger<DownloadSessionHandler>());

            SemaphoreSlim semaphore = new SemaphoreSlim(0, fileDownloadOptions.Length);

            handler.OnDownloadCompleted += (sender, report) => {
                semaphore.Release();
            };

            Dictionary<string, bool> status = new Dictionary<string, bool>();
            foreach(var fileDownloadOption in fileDownloadOptions) {
                status.Add(fileDownloadOption.File.ToUpper(), false);
            }

            handler.OnDownloadProgress += (sender, report) => {
                status[report.Configuration.File.ToUpper()] = true;
            };

            await handler.StartSessionAsync(downloadSession);

            semaphore.Wait(10000);

            Assert.That(
                () => status.All(x => x.Value != false));

            Assert.AreEqual(1, handler.StartSessionAsyncCalls);
        }
    }
}
