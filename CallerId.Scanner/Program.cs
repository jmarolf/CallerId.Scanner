using System;
using CallerId.Scanner.Lib;
using CallerId.Scanner.Lib.Compiler;
using CommandLine;
using Newtonsoft.Json;

namespace CallerId.Scanner
{
    class Program
    {
        private static readonly IoCConfig IocConfig = new IoCConfig();

        /// <summary>
        /// Compile .sln
        /// Get changed files
        /// Send rich file object per changed file
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(async o =>
                {
                    var msbuildService = IocConfig.GetInstance<IMSBuildService>();
                    msbuildService.Initialize();

                    var dependenciesGenerator = IocConfig.GetInstance<IDependenciesInFileGenerator>();
                    var dependencies = await dependenciesGenerator.GetFromProjectAsync(o.SolutionPath, o.Document);

                    foreach (var dependency in dependencies)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(dependency));
                    }
                });
        }
    }
}
