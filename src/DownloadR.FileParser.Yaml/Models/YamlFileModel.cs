using System.Collections.Generic;

using DownloadR.Session;

namespace DownloadR.FileParser.Yaml.Models {
    internal class YamlFileModel {
        public YamlConfigurationSettings Config { get; set; }

        public List<DownloadFileOptions> Downloads { get; set; }

    }
}
