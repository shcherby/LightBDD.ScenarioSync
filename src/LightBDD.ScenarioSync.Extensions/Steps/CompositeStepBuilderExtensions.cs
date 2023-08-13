using LightBDD.Framework;
using LightBDD.Framework.Scenarios;

namespace LightBDD.ScenarioSync.Extensions.Steps;

internal static class CompositeStepBuilderExtensions
{
    public static Task<CompositeStep> BuildAsync<TContext>(this ICompositeStepBuilder<TContext> compositeStepBuilder)
        => Task.FromResult(compositeStepBuilder.Build());
}
