using CommandLine;

namespace CallerId.Scanner
{
    public class Options
    {
        [Option('p', "path", Required = true, HelpText = "Path to solution file.")]
        public string SolutionPath { get; set; }

        [Option('d', "document", Required = true, HelpText = "Name of changed document.")]
        public string Document { get; set; }
    }
}
