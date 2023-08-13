using System.Text;
using LightBDD.ScenarioSync.Core.Entities.Parameters;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

public class StepNameParameterExpectationRenderer
{
    private readonly IReadOnlyCollection<TestStepParameter> _parameters;

    public StepNameParameterExpectationRenderer(IReadOnlyCollection<TestStepParameter> parameters)
    {
        _parameters = parameters;
    }

    public string Render()
    {
        var builder = new StringBuilder();
        var expectations = _parameters.Where(p => !string.IsNullOrEmpty(p.Value.Expectation));
        foreach (TestStepParameter param in expectations)
        {
            builder.AppendLine($"Param: {param.Name}, Expectation: {param.Value.Expectation}");
        }

        return builder.ToString();
    }
}