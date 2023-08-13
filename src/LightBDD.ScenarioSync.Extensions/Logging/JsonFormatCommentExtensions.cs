using LightBDD.Framework;
using Newtonsoft.Json;

namespace LightBDD.ScenarioSync.Extensions.Logging;

public static class JsonFormatCommentExtensions
{
    public static void Log(this string message, string title = "")
    {
        PrintTitle(title);
        StepExecution.Current.Comment(message);
    }

    public static TData Log<TData>(this TData data, string title = "")
    {
        PrintTitle(title);
        StepExecution.Current.Comment(JsonConvert.SerializeObject(data, Formatting.Indented));
        return data;
    }

    private static void PrintTitle(string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            StepExecution.Current.Comment($"{title}:");
        }
    }
}
