using System;
using System.Threading;
using System.Threading.Tasks;
using DownloadR.BaseTestProject;
using DownloadR.Impl.UnitTestProject.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace DownloadR.Impl.UnitTestProject
{
    public class DownloadFileTaskTests {
        [Test]
        public void Verify_ThrowsException_When_IDownloadFileClient_Returns_Null() {
            DownloadFileConfig config = DownloadFileConfigBuilder.GetValid();

            Mock<DownloadFileTask> t =
                new Mock<DownloadFileTask>(MockBehavior.Default, config, NullLogger<DownloadFileTask>.Instance);
            t.Protected()
                .Setup<IDownloadFileClient>("BuildClient")
                .Returns(() => null);

            Assert.Throws<ApplicationException>(() => { t.Object.GetFileSize(); });
        }

        [Test]
        public void Verify_GetFileSize() {
            DownloadFileConfig config = DownloadFileConfigBuilder.GetValid();

            Mock<DownloadFileTask> t =
                new Mock<DownloadFileTask>(MockBehavior.Default, config, NullLogger<DownloadFileTask>.Instance);
            t.Protected()
                .Setup<IDownloadFileClient>("BuildClient")
                .Returns(() => new DownloadFileClientMock());

            Assert.AreEqual(DownloadFileClientMock.FILE_SIZE, t.Object.GetFileSize());
        }

        [Test]
        public async Task Verify_Observer_Is_Called_when_StartDownload() {
            DownloadFileConfig config = DownloadFileConfigBuilder.GetValid();

            DownloadFileClientMock downloadFileClientMock = new DownloadFileClientMock();

            Mock<DownloadFileTask> task =
                new Mock<DownloadFileTask>(MockBehavior.Default, config, NullLogger<DownloadFileTask>.Instance);
            task.Protected()
                .Setup<IDownloadFileClient>("BuildClient")
                .Returns(() => downloadFileClientMock);

            Mock<IObserver<IFileDownloadProgress>> observer = new Mock<IObserver<IFileDownloadProgress>>();

            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, 1);

            observer.Setup(x => x.OnCompleted()).Callback(() => {
                semaphoreSlim.Release();
            });

            task.Object.Subscribe(observer.Object);

            await task.Object.StartDownloadAsync();

            await semaphoreSlim.WaitAsync(5000);

            Assert.IsTrue(downloadFileClientMock.StartDownloadFromUriWasCalled);

            observer.Verify(x => x.OnNext(It.IsAny<IFileDownloadProgress>()), Times.AtLeastOnce);
            observer.Verify(x => x.OnCompleted(), Times.AtLeastOnce);
        }
    }
}