using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DownloadR.Session;

using Microsoft.Extensions.Logging;

namespace DownloadR.FileParserCore {
    public class DownloadSessionFileParser {
        private readonly ILogger<DownloadSessionFileParser> _logger;
        private readonly ConcurrentDictionary<string, List<IDownloadSessionFileParser>> _supportedReaders = new();

        public DownloadSessionFileParser(ILogger<DownloadSessionFileParser> logger) {
            this._logger = logger;
        }

        /// <summary>
        /// Adds a <see cref="IDownloadSessionFileParser"/>
        /// </summary>
        /// <param name="reader"></param>
        public void AddReader(IDownloadSessionFileParser reader) {
            string key = reader.SupportedFileExtension.ToUpper();

            this._supportedReaders.AddOrUpdate(key,
                _ => new List<IDownloadSessionFileParser> { reader },
                (_, list) => {
                    list.Add(reader);
                    return list;
                });

            this._logger.LogInformation("FileParser added for extension '{0}': {1}", key, reader.GetType().FullName);
        }

        /// <summary>
        /// Loads a <see cref="DownloadSession"/> from a <see cref="FileStream"/>
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public DownloadSessionLoadResult LoadDownloadSession(FileStream fileStream) {

            if(fileStream == null) throw new ArgumentNullException(nameof(fileStream));

            var extension = this.clearFileExtension(Path.GetExtension(fileStream.Name).ToUpper());

            this._logger.LogTrace("Looking for {0} for extension '{1}'", nameof(IDownloadSessionFileParser), extension);

            return this.loadDownloadSession(fileStream, extension);
        }

        private DownloadSessionLoadResult loadDownloadSession(FileStream fileStream, string extension) {
            List<Exception> exceptionsFromReader = new List<Exception>();

            if(this._supportedReaders.ContainsKey(extension)) {
                this._logger.LogTrace("{0} for extension '{1}' found", nameof(IDownloadSessionFileParser), extension);

                foreach(IDownloadSessionFileParser reader in this._supportedReaders[extension]) {
                    try {
                        var result = reader.LoadDownloadSession(fileStream);

                        this._logger.LogTrace("{0} loaded by {1} for extension '{1}'",
                            nameof(DownloadSession),
                            nameof(IDownloadSessionFileParser),
                            extension);

                        return new DownloadSessionLoadResult(result);
                    }
                    catch(Exception e) {
                        exceptionsFromReader.Add(e);
                    }
                }
            }

            Exception exception = null;

            if(exceptionsFromReader.Any()) {
                exception = exceptionsFromReader.Count == 1
                    ? exceptionsFromReader[0]
                    : new AggregateException(
                        $"{exceptionsFromReader.Count} errors while reading the file '{fileStream.Name}'. Check inner exceptions for details",
                        exceptionsFromReader);
            }

            return new DownloadSessionLoadResult(exception ??
                                                 new KeyNotFoundException(
                                                     $"No {nameof(IDownloadSessionFileParser)} found for extensions '{extension}'"));
        }

        private string clearFileExtension(string extension) {
            if(!string.IsNullOrEmpty(extension?.Trim()) && extension.Length > 0 && extension.StartsWith('.'))
                return extension.Substring(1);

            return extension;
        }
    }
}
