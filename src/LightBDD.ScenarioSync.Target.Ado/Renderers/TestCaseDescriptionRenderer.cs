using System.Text;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Entities.Parameters;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

public static class TestCaseDescriptionRenderer
{
    public static string Render(TestCase testCase)
    {
        var writer = new StringWriter();
        writer.Write("Scenario: ");
        writer.Write(testCase.Name);
        FormatLabels(writer, testCase.Tags);
        writer.WriteLine();

        var commentBuilder = new StringBuilder();
        var attachmentBuilder = new StringBuilder();
        foreach (var step in testCase.Steps)
            FormatStep(writer, step, commentBuilder, attachmentBuilder);
        FormatComments(writer, commentBuilder);
        writer.WriteLine();
        FormatAttachments(writer, attachmentBuilder);

        return writer.ToString();
    }


    private static void FormatLabels(TextWriter writer, IEnumerable<string> labels)
    {
        var first = true;

        foreach (var label in labels)
        {
            if (first)
            {
                writer.Write(' ');
                first = false;
            }

            writer.Write("[");
            writer.Write(label);
            writer.Write("]");
        }
    }

    private static void FormatStep(TextWriter writer, TestStep step, StringBuilder commentBuilder, StringBuilder attachmentBuilder, int indent = 0)
    {
        var stepIndent = new string('\t', indent);
        writer.Write(stepIndent);
        writer.Write("Step ");
        writer.Write(step.GroupPrefix);
        writer.Write(step.Number);
        writer.Write(": ");
        writer.Write(step.Name);
        writer.WriteLine();
        foreach (var parameterResult in step.Parameters)
            FormatParameter(writer, parameterResult, stepIndent);
        CollectComments(step, commentBuilder);
        CollectAttachments(step, attachmentBuilder);
        foreach (var subStep in step.GetSubSteps())
            FormatStep(writer, subStep, commentBuilder, attachmentBuilder, indent + 1);
    }

    private static void FormatParameter(TextWriter writer, TestStepParameter parameterResult, string stepIndent)
    {
        if (parameterResult.Table.Columns.Any())
        {
            writer.Write(stepIndent);
            writer.Write(parameterResult.Name);
            writer.WriteLine(":");
            new TextTableParameterRenderer(parameterResult.Table).Render(writer, stepIndent);
            writer.WriteLine();
        }

        if (parameterResult.Tree.Nodes.Any())
        {
            writer.Write(stepIndent);
            writer.Write(parameterResult.Name);
            writer.WriteLine(":");
            new TextTreeParameterRenderer(parameterResult.Tree).Render(writer, stepIndent);
            writer.WriteLine();
        }
    }

    private static void CollectAttachments(TestStep step, StringBuilder attachmentBuilder)
    {
        foreach (var attachment in step.FileAttachments)
        {
            attachmentBuilder.Append("\tStep ").Append(step.GroupPrefix).Append(step.Number)
                .Append(": ")
                .Append(attachment.Name)
                .Append(" - ")
                .AppendLine(attachment.RelativePath);
        }
    }

    private static void FormatAttachments(TextWriter writer, StringBuilder attachmentBuilder)
    {
        if (attachmentBuilder.Length == 0)
            return;
        writer.WriteLine("Attachments:");
        writer.Write(attachmentBuilder);
    }

    private static void CollectComments(TestStep step, StringBuilder commentBuilder)
    {
        foreach (var comment in step.Comments)
        {
            commentBuilder.Append("\tStep ").Append(step.GroupPrefix).Append(step.Number)
                .Append(": ")
                .AppendLine(comment.Replace(Environment.NewLine, Environment.NewLine + "\t\t"));
        }
    }

    private static void FormatComments(TextWriter writer, StringBuilder commentBuilder)
    {
        if (commentBuilder.Length == 0)
            return;
        writer.WriteLine("Comments:");
        writer.Write(commentBuilder);
    }
}