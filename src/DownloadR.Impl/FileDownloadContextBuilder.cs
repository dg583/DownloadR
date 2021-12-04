using System.Collections.Generic;
using System.IO;

using DownloadR.Factories;
using DownloadR.Session;

namespace DownloadR {
    internal class FileDownloadContextBuilder {
        private readonly DownloadHandlerOptions _options;
        private readonly IDownloadTaskFactory _downloadTaskBuilder;

        public FileDownloadContextBuilder(DownloadHandlerOptions options, IDownloadTaskFactory downloadTaskBuilder) {
            this._options = options;
            this._downloadTaskBuilder = downloadTaskBuilder;
        }

        public IEnumerable<FileDownloadContext> Build(DownloadSession downloadSession) {
            return this.createFileDownloadContextsFromDownloadSession(downloadSession);
        }

        private IEnumerable<FileDownloadContext> createFileDownloadContextsFromDownloadSession(DownloadSession downloadSession) {

            string path = this.getDownloadPath(downloadSession);

            this.createOutputDirectory(path);

            foreach(DownloadFileOptions fileDownloadOptions in downloadSession.Configuration) {
                var context = createFileDownloadContext(fileDownloadOptions, path);
                yield return context;
            }
        }

        private FileDownloadContext createFileDownloadContext(DownloadFileOptions fileDownloadOptions, string path) {

            string filePath = Path.Combine(path, fileDownloadOptions.File);

            if(File.Exists(filePath)) {
                if(fileDownloadOptions.Overwrite)
                    File.Delete(filePath);
                //else {
                //    throw new Exception($"The file '{filePath}'already exists and should not be deleted");
                //    //TODO: What, if file exists?
                //}
            }

            var downloadFileConfig = new DownloadFileConfig(filePath, fileDownloadOptions.Url);

            IDownloadFileTask task = this.buildDownloadFileTask(downloadFileConfig);

            var context = new FileDownloadContext(fileDownloadOptions, downloadFileConfig, task);

            return context;
        }
        protected virtual IDownloadFileTask buildDownloadFileTask(DownloadFileConfig downloadFileConfig) {
            //Possible seam for testing; instead of using the factory
            return this._downloadTaskBuilder.CreateDownloadFileTask(downloadFileConfig);
        }

        protected virtual string getDownloadPath(DownloadSession downloadSession) {
            // Check because linux has different rootPath-notation than win
            if(downloadSession.Settings.DownloadDirectory.IsFullPath()) {
                return downloadSession.Settings.DownloadDirectory;
            }

            return Path.GetFullPath($"{this._options.WorkingDirectory}{downloadSession.Settings.DownloadDirectory}");
        }

        private void createOutputDirectory(string path) {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if(!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Could not create output folder '{path}'");
        }

    }
}