using System;

using DownloadR.Session;

using Moq;

namespace DownloadR.Impl.UnitTestProject {

    public class Factory {

        public static Uri GetUriToFile() {
            string getFileSizeEndpoint = "https://get.filesize.dev";
            Uri getFileSizeUri = new Uri(getFileSizeEndpoint);
            return getFileSizeUri;
        }

        public static DownloadFileConfig GetDownloadFileConfig() {
            return new DownloadFileConfig("c:\\dummy.file", GetUriToFile().AbsoluteUri);
        }

        public static FileDownloadContext GetFileDownloadContext() {
            DownloadFileOptions downloadConfiguration = new DownloadFileOptions();

            Mock<IDownloadFileTask> downloadFileTask = new Mock<IDownloadFileTask>();

            var cfg = GetDownloadFileConfig();
            FileDownloadContext result = new FileDownloadContext(downloadConfiguration, cfg, downloadFileTask.Object);

            return result;

        }
    }
}
