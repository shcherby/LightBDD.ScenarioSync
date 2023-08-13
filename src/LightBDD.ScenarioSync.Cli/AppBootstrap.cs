using System.Reflection;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Sync;
using Microsoft.Extensions.DependencyInjection;

namespace LightBDD.ScenarioSync.Cli;

public class AppBootstrap
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AppArguments _arguments;
    private Startup _startup;
    private Action<IServiceCollection> _overridesFunc;

    public AppBootstrap(AppArguments arguments)
    {
        _arguments = arguments;
    }

    public AppBootstrap WithStartup(Startup startup)
    {
        _startup = startup;
        return this;
    }

    public AppBootstrap WithOverrides(Action<IServiceCollection> overridesFunc)
    {
        _overridesFunc = overridesFunc;
        return this;
    }

    public async Task RunAsync<TAppCommand>() where TAppCommand : IAppCommand
    {
        var serviceCollections = new ServiceCollection();
        _startup?.ConfigureServices(serviceCollections);
        serviceCollections.AddScoped<IAppExecutionContext>(_ => new AppExecutionContext(_arguments));

        _overridesFunc?.Invoke(serviceCollections);

        var serviceProvider = serviceCollections.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        var command = serviceProvider.GetRequiredService<TAppCommand>();
        await command.RunAsync(_arguments);
    }
}