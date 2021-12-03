using System;
using System.Collections.Generic;

using DownloadR.FileParser.Yaml.Models;
using DownloadR.Session;

namespace DownloadR.FileParser.Yaml {
    internal static class YamlFileModelExtensions {

        /// <summary>
        /// Converts <see cref="YamlFileModel"/> to <see cref="DownloadSession"/>
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        public static DownloadSession ToDownloadConfiguration(this YamlFileModel fileModel) {

            if(fileModel == null)
                throw new ArgumentNullException(nameof(fileModel));

            if(fileModel.Config == null) {
                throw new ArgumentNullException(nameof(fileModel.Config));
            }

            var settings = new DownloadSessionConfiguration(fileModel.Config.DownloadDir, fileModel.Config.ParallelDownloads);

            return new DownloadSession(settings, fileModel.Downloads ?? new List<DownloadFileOptions>());
        }
    }
}
