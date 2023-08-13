using LightBDD.ScenarioSync.Core;
using LightBDD.ScenarioSync.Core.App.Commands;
using LightBDD.ScenarioSync.Source;
using LightBDD.ScenarioSync.Target.Ado;
using Microsoft.Extensions.DependencyInjection;

namespace LightBDD.ScenarioSync.Cli;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollections)
    {
        serviceCollections.AddScoped<PushAppCommand>();
        serviceCollections.AddScoped<CleanAppCommand>();
        SourceDependencyContainer.RegisterServices(serviceCollections);
        TargetAdoDependencyContainer.RegisterServices(serviceCollections);
        CoreDependencyContainer.RegisterServices(serviceCollections);
    }
}