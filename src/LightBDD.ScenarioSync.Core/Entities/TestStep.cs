using LightBDD.ScenarioSync.Core.Entities.Parameters;
using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities;

public record TestStep
{
    private IEnumerable<TestStep> _subSteps = new List<TestStep>();

    public TestStep(
        string name,
        int number,
        string groupPrefix,
        IEnumerable<TestStepParameter> testStepParameters,
        IEnumerable<string>? comments = null,
        IEnumerable<FileAttachment>? fileAttachments = null,
        IEnumerable<TestStep>? subSteps = null
    )
    {
        Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
        Number = number;
        GroupPrefix = groupPrefix;

        fileAttachments ??= Enumerable.Empty<FileAttachment>();
        FileAttachments = fileAttachments.ToList();

        comments ??= Enumerable.Empty<string>();
        Comments = comments.ToList();

        testStepParameters ??= Enumerable.Empty<TestStepParameter>();
        Parameters = testStepParameters.ToList();

        _subSteps = subSteps ?? new List<TestStep>();
    }

    public string Name { get; set; }

    public int Number { get; }

    public string GroupPrefix { get; }

    public IReadOnlyCollection<string> Comments { get; }

    public IReadOnlyCollection<FileAttachment> FileAttachments { get; }

    public IReadOnlyCollection<TestStepParameter> Parameters { get; }

    public IReadOnlyCollection<TestStep> GetSubSteps() => _subSteps.ToList();
}