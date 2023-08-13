using LightBDD.ScenarioSync.Core.Sync;
using Microsoft.Extensions.DependencyInjection;

namespace LightBDD.ScenarioSync.Core;

public class CoreDependencyContainer
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITestSuitesSynchronizer, TestSuitesSynchronizer>();
    }
}
