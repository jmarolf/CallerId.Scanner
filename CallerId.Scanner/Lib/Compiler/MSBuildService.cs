using System;
using System.Linq;
using Microsoft.Build.Locator;

namespace CallerId.Scanner.Lib.Compiler
{
    internal interface IMSBuildService
    {
        void Initialize();
    }

    class MsBuildService : IMSBuildService
    {
        private readonly ILogger _logger;

        public MsBuildService(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            VisualStudioInstance[] instances = GetVisualStudioInstances();
            if (instances.Length == 0)
            {
                return;
            }

            VisualStudioInstance instance = instances[0];
            RegisterVisualStudioInstance(instance);
        }

        private VisualStudioInstance[] GetVisualStudioInstances()
        {
            VisualStudioInstance[] instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            if (instances.Length == 0)
            {
                _logger.Log("No MSBuild instances found.");
                return Array.Empty<VisualStudioInstance>();
            }

            _logger.Log("The following MSBuild instances have been discovered:");

            for (int i = 0; i < instances.Length; i++)
            {
                VisualStudioInstance instance = instances[i];
                _logger.Log($"    {i + 1}. {instance.Name} ({instance.Version})");
            }

            return instances;
        }

        private void RegisterVisualStudioInstance(VisualStudioInstance instance)
        {
            MSBuildLocator.RegisterInstance(instance);

            _logger.Log("Registered first MSBuild instance:");
            _logger.Log($"    Name: {instance.Name}");
            _logger.Log($"    Version: {instance.Version}");
            _logger.Log($"    VisualStudioRootPath: {instance.VisualStudioRootPath}");
            _logger.Log($"    MSBuildPath: {instance.MSBuildPath}");
            _logger.Log(string.Empty);
        }
    }

}
