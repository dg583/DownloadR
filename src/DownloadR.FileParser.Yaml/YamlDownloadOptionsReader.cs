using System.IO;

using DownloadR.FileParser.Yaml.Models;
using DownloadR.FileParserCore;
using DownloadR.Session;

using YamlDotNet.Serialization;

namespace DownloadR.FileParser.Yaml {

    /// <summary>
    /// <see cref=""/>
    /// </summary>
    public class YamlDownloadOptionsReader : IDownloadSessionFileParser {
        /// <summary>
        /// 
        /// </summary>
        public string SupportedFileExtension => "yaml";

        /// <summary>
        /// Converts stream to <see cref="DownloadSession"/>
        /// </summary>
        /// <param name="fromStream"></param>
        /// <returns></returns>
        public DownloadSession LoadDownloadSession(Stream fromStream) {
            DownloadSession downloadOptions = this.deserialize(fromStream);
            return downloadOptions;
        }

        private DownloadSession deserialize(Stream stream) {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(
                    new CamelCaseUnderscoreNamingConvention()
                )
                .Build();

            TextReader reader = new StreamReader(stream);

            YamlFileModel yamlFileModel = deserializer.Deserialize<YamlFileModel>(reader);

            return yamlFileModel.ToDownloadConfiguration();
        }
    }
}
