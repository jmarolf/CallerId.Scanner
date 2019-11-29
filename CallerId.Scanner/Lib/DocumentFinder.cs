using System;
using System.Linq;
using System.Threading.Tasks;
using CallerId.Scanner.Lib.Compiler;
using Microsoft.CodeAnalysis;

namespace CallerId.Scanner.Lib
{
    internal interface IDocumentFinder
    {
        Task<Document> FindDocument(string projectPath, string documentName);
    }
    class DocumentFinder : IDocumentFinder
    {
        private readonly IWorkspaceService _workspaceService;

        public DocumentFinder(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        public async Task<Document> FindDocument(string pathToSolution, string documentName)
        {
            var solution = await _workspaceService.GetSolutionAsync(pathToSolution);

            foreach (var project in solution.Projects)
            {
                var doc = project.Documents.FirstOrDefault(d => d.Name == documentName);
                if (doc != null)
                {
                    return doc;
                }
            }
            throw new Exception("Could not find document!");
        }
    }
}
