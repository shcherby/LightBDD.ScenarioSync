namespace LightBDD.ScenarioSync.Target.Ado.Renderers
{
    public interface IStepParameterRenderer
    {
        void Render(TextWriter writer, string stepIntend);
    }
}