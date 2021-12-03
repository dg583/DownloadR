using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DownloadR.FileParser.Yaml {
    /// <summary>
    /// Combines CamelCase- and Underscored-Naming conventions
    /// </summary>
    internal class CamelCaseUnderscoreNamingConvention : INamingConvention {
        public string Apply(string value) {
            CamelCaseNamingConvention camelCaseNamingConvention = new CamelCaseNamingConvention();
            UnderscoredNamingConvention underscoredNamingConvention = new UnderscoredNamingConvention();
            value = camelCaseNamingConvention.Apply(value);

            return underscoredNamingConvention.Apply(value);
        }
    }
}