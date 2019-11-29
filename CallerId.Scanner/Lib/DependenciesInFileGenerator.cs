using System.Collections.Generic;
using System.Threading.Tasks;
using CallerId.Models;

namespace CallerId.Scanner.Lib
{
    public interface IDependenciesInFileGenerator
    {
        Task<IEnumerable<Dependency>> GetFromProjectAsync(string pathToProj, string documentName);
    }

    internal class DependenciesInFileGenerator : IDependenciesInFileGenerator
    {
        private readonly IDocumentFinder _documentFinder;

        public DependenciesInFileGenerator(IDocumentFinder documentFinder)
        {
            _documentFinder = documentFinder;
        }
        public async Task<IEnumerable<Dependency>> GetFromProjectAsync(string pathToProj, string documentName)
        {
            var doc = await _documentFinder.FindDocument(pathToProj, documentName);

            var syntaxTree = await doc.GetSyntaxRootAsync();
            var semanticModel = await doc.GetSemanticModelAsync();

            return new Dependency[] { };
        }
    }
}
