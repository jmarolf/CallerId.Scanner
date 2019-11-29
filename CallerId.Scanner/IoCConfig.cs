using CallerId.Scanner.Lib;
using CallerId.Scanner.Lib.Compiler;
using SimpleInjector;

namespace CallerId.Scanner
{
    public interface IIoCConfig
    {
        T GetInstance<T>() where T : class;
    }

    public class IoCConfig : IIoCConfig
    {
        public Container Container;

        public IoCConfig()
        {
            Container = new Container();
            Configure();
        }

        private void Configure()
        {
            Container.Register<ILogger, ConsoleLogger>();
            Container.Register<IMSBuildService, MsBuildService>();

            Container.Register<IDependenciesInFileGenerator,DependenciesInFileGenerator>();
            Container.Register<IDocumentFinder, DocumentFinder>();
            Container.Register<IWorkspaceService, WorkspaceService>();
        }

        public T GetInstance<T>() where T : class
        {
            return Container.GetInstance<T>();
        }
    }
}
