using LightBDD.ScenarioSync.Core.Sync.Target;
using Microsoft.Extensions.DependencyInjection;

namespace LightBDD.ScenarioSync.Target.Ado;

public class TargetAdoDependencyContainer
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITmsClient, TmsAdoClient>();
    }
}
