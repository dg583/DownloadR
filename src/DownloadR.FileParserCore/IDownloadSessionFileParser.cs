using System.IO;

using DownloadR.Session;

namespace DownloadR.FileParserCore {
    public interface IDownloadSessionFileParser {

        /// <summary>
        /// The file extension that the reader can read
        /// </summary>
        public string SupportedFileExtension { get; }

        /// <summary>
        /// Loads <see cref="DownloadSession"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="fromStream"></param>
        /// <returns></returns>
        DownloadSession LoadDownloadSession(Stream fromStream);
    }
}