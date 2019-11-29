using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CallerId.Scanner.Lib.Compiler
{
    public interface IWorkspaceService
    {
        Task<Solution> GetSolutionAsync(string pathToSolution);
        Task<Project> GetProjectAsync(string pathToProject);
    }

    internal class WorkspaceService : IWorkspaceService
    {
        private readonly ILogger _logger;
        public MSBuildWorkspace Workspace { get; }

        public WorkspaceService(ILogger logger)
        {
            _logger = logger;
            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                // This property ensures that XAML files will be compiled in the current AppDomain
                // rather than a separate one. Any tasks isolated in AppDomains or tasks that create
                // AppDomains will likely not work due to https://github.com/Microsoft/MSBuildLocator/issues/16.
                { "AlwaysCompileMarkupFilesInSeparateDomain", bool.FalseString }
            };

            Workspace = MSBuildWorkspace.Create(properties);
            Workspace.WorkspaceFailed += WorkspaceFailed;
        }

        private void WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
            if (e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
            {
                _logger.Log(e.Diagnostic.Message);
            }
            else
            {
                _logger.Log(e.Diagnostic.Message);
            }
        }


        public async Task<Project> GetProjectAsync(string projectFilePath)
        {
            LogHeader();

            Stopwatch watch = Stopwatch.StartNew();
            Project project = await Workspace.OpenProjectAsync(projectFilePath, new LoaderProgress(_logger));

            watch.Stop();
            _logger.Log($"\r\nProject opened: {watch.Elapsed:m\\:ss\\.fffffff}");
            return project;
        }

        private void LogHeader()
        {
            _logger.Log("Operation       Elapsed Time    Project");
        }

        private class LoaderProgress : IProgress<ProjectLoadProgress>
        {
            private readonly ILogger _logger;

            public LoaderProgress(ILogger logger)
            {
                _logger = logger;
            }

            public void Report(ProjectLoadProgress loadProgress)
            {
                string projectDisplay = Path.GetFileName(loadProgress.FilePath);

                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                _logger.Log($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }

        public async Task<Solution> GetSolutionAsync(string pathToSolution)
        {
            LogHeader();

            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                Solution solution = await Workspace.OpenSolutionAsync(pathToSolution);

                watch.Stop();
                _logger.Log($"\r\nSolution opened: {watch.Elapsed:m\\:ss\\.fffffff}");

                return solution;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
