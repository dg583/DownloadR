using System;
using System.Threading.Tasks;
using DownloadR.Factories;
using DownloadR.Impl.UnitTestProject.Fakes;
using DownloadR.Session;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using NUnit.Framework;

namespace DownloadR.Impl.UnitTestProject {

    public class DefaultSessionHandlerBuilderTests {
        [Test]
        public void Verify_() {
            ILoggerFactory loggerFactory = NullLoggerFactory.Instance;

            Mock<IDownloadFileTask> downloadFileTask = new Mock<IDownloadFileTask>();

            Mock<IDownloadTaskFactory> downloadTaskFactory = new Mock<IDownloadTaskFactory>();
            downloadTaskFactory
                .Setup(x => x.CreateDownloadFileTask(It.IsAny<DownloadFileConfig>()))
                .Returns(downloadFileTask.Object);

            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IDownloadTaskFactory)))
                .Returns(downloadTaskFactory.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(ILogger<DownloadSessionHandler>)))
                .Returns(NullLoggerFactory.Instance.CreateLogger<DownloadSessionHandler>());


            DefaultSessionHandlerFactory builder = new DefaultSessionHandlerFactory(serviceProvider.Object);

            string workingDir = TestContext.CurrentContext.WorkDirectory;

            DownloadHandlerOptions downloadHandlerOptions = new DownloadHandlerOptions(workingDir);
            IDownloadSessionHandler downloadSessionHandler = builder.CreateDownloadSessionHandler(downloadHandlerOptions);

            Assert.IsNotNull(downloadSessionHandler);
        }
    }

    public class DownloadManagerTests {
        [Test]
        public void Verify_Can_Instantiate() {
            Mock<IDownloadTaskFactory> downloadTaskBuilder = new Mock<IDownloadTaskFactory>();
            downloadTaskBuilder.Setup(x => x.CreateDownloadFileTask(It.IsAny<DownloadFileConfig>()))
                .Returns(() => new DownloadFileTaskStub());


            Mock<ISessionHandlerFactory> sessionHandlerBuilder = new Mock<ISessionHandlerFactory>();

            string workingDir = TestContext.CurrentContext.WorkDirectory;

            DownloadManager downloadManager = new DownloadManager(
                new DownloadManagerOptions { WorkingDirectory = workingDir },
                sessionHandlerBuilder.Object,
                NullLoggerFactory.Instance.CreateLogger<DownloadManager>());

            Assert.IsNotNull(downloadManager);
        }

        [Test]
        public void Verify_Works() {
            Mock<IDownloadTaskFactory> downloadTaskBuilder = new Mock<IDownloadTaskFactory>();
            downloadTaskBuilder.Setup(x => x.CreateDownloadFileTask(It.IsAny<DownloadFileConfig>()))
                .Returns(() => new DownloadFileTaskStub());

            string workingDir = TestContext.CurrentContext.WorkDirectory;
            var options = new DownloadHandlerOptions(workingDir);

            DownloadSessionHandlerMock downloadSessionHandler = new DownloadSessionHandlerMock(options,
                downloadTaskBuilder.Object, new NullLogger<DownloadSessionHandler>());

            Mock<ISessionHandlerFactory> sessionHandlerBuilder = new Mock<ISessionHandlerFactory>();
            sessionHandlerBuilder
                .Setup(x => x.CreateDownloadSessionHandler(It.IsAny<DownloadHandlerOptions>()))
                .Returns(downloadSessionHandler);



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

            DownloadSession session = new DownloadSession(downloadSessionConfiguration, fileDownloadOptions);

            DownloadManager downloadManager = new DownloadManager(
                new DownloadManagerOptions { WorkingDirectory = workingDir },
                sessionHandlerBuilder.Object,
                NullLoggerFactory.Instance.CreateLogger<DownloadManager>());

            Task task = downloadManager.StartSessionAsync(session, null);
            task.Wait();

            sessionHandlerBuilder.Verify(
                x => x.CreateDownloadSessionHandler(It.IsAny<DownloadHandlerOptions>()),
                Times.Once
            );

            Assert.AreEqual(1, downloadSessionHandler.StartSessionAsyncCalls);

            Assert.Pass();
        }
    }
}
