namespace DownloadR.FileParser.Yaml.Models
{
    internal class YamlConfigurationSettings {
    
        /// <summary>
        /// Number of parallel downloads
        /// </summary>
        public int ParallelDownloads { get; set; }
        public string DownloadDir { get; set; }
    }
}