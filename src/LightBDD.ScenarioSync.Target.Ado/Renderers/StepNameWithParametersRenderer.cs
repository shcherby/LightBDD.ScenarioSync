using LightBDD.ScenarioSync.Core.Entities;
using System.Text;
using System.Text.Encodings.Web;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

public class StepNameWithParametersRenderer
{
    private readonly TestStep _step;

    public StepNameWithParametersRenderer(TestStep step)
    {
        _step = step;
    }

    public string Render(int indent = 0)
    {
        var sb = new StringBuilder();
        sb.Append($"<span>{_step.GroupPrefix}{_step.Number}: {HtmlEncoder.Default.Encode(_step.Name)}</span>");
        var parameters = _step.Parameters.ToList();
        for (int i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            if (parameter.Table.Columns.Any())
            {
                sb.Append($"<div><strong>{HtmlEncoder.Default.Encode(parameter.Name)}:</strong></div>");
                sb.Append(new HtmlTableParameterRenderer(parameter.Table).Render());
            }

            if (parameter.Tree.Nodes.Any())
            {
                sb.Append($"<div><strong>{HtmlEncoder.Default.Encode(parameter.Name)}:</strong></div>");
                sb.Append($"<div>{new HtmlTreeParameterRenderer(parameter.Tree).Render()}</div>");
            }

            if (i < parameters.Count - 1)
            {
                sb.Append("<span>&nbsp;</span>");
            }
        }

        return sb.ToString();
    }
}