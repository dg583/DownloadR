using CommandLine;

namespace DownloadR.ConsoleApp
{
    public class Options {
        [Option('v', "verbose", HelpText = "Logs verbose information")]
        public bool Verbose { get; set; }

        [Option("dryrun")]
        public bool DryRun { get; set; }

        [Value(0)]
        public string Command { get; set; }

        [Option('f', "file", Required = true, HelpText = "Path to download definition file")]
        public string SessionFile { get; set; }

        [Option("parallel-downloads", HelpText = "Sets or override parallel downloads")]
        public int? ParallelDownload { get; set; }
    }
}