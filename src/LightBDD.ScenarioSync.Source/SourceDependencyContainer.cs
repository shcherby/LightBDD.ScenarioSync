using LightBDD.ScenarioSync.Core.Sync.Source;
using LightBDD.ScenarioSync.Source.XmlParser;
using Microsoft.Extensions.DependencyInjection;

namespace LightBDD.ScenarioSync.Source;

public static class SourceDependencyContainer
{
    public static void RegisterServices(IServiceCollection serviceCollection) {

        serviceCollection.AddSingleton<ITestCasesSource, TestCasesSource>();
        serviceCollection.AddSingleton<FeaturesReportXmlParser>();
    }
}