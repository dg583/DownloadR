using System;

using DownloadR.EventArguments;

using NUnit.Framework;

namespace DownloadR.Impl.UnitTestProject {
    public class DownloadFileNotificationEventArgsTests {
        [Test]
        public void Verify_DownloadFileNotificationEventArgs_FactoryMethods() {
            FileDownloadContext context = Factory.GetFileDownloadContext();
            DownloadFileNotificationEventArgs args = DownloadFileNotificationEventArgs.BuildForCompleted(context);

            Assert.AreEqual(FileDownloadStatusType.Completed, args.Status);
            Assert.AreSame(context, args.Context);
            Assert.IsNotNull(args.Progress);
            Assert.IsNull(args.Exception);

            FileDownloadProgress progress = new FileDownloadProgress(100, 0, 0);
            args = DownloadFileNotificationEventArgs.BuildForProgress(context, progress);

            Assert.AreEqual(FileDownloadStatusType.Downloading, args.Status);
            Assert.AreSame(context, args.Context);
            Assert.IsNull(args.Exception);
            Assert.AreEqual(progress, args.Progress);

            Exception ex = new Exception("Test");
            args = DownloadFileNotificationEventArgs.BuildForException(context, ex);

            Assert.AreEqual(FileDownloadStatusType.Failed, args.Status);
            Assert.AreSame(context, args.Context);
            Assert.IsNotNull(args.Progress);
            Assert.AreEqual(ex, args.Exception);

        }
    }
}