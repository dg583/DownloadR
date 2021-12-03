using CommandLine;

namespace DownloadR.ConsoleApp
{
    public static class OptionsParser {
        public static ParserResult<Options> ParseArgs(this string[] args) {
            return Parser.Default.ParseArguments<Options>(args);
        }
    }
}