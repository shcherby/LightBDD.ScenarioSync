using LightBDD.ScenarioSync.Core.Entities;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

public class StepNameRenderer
{
    public static string Render(TestStep step)
    {
        var writer = new StringWriter();
        writer.Write(step.GroupPrefix);
        writer.Write(step.Number);
        writer.Write(": ");
        writer.Write(step.Name);

        return writer.ToString();
    }
}