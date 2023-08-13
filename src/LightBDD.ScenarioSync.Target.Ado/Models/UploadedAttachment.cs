using Microsoft.VisualStudio.Services.WebApi.Patch;

namespace LightBDD.ScenarioSync.Target.Ado.Models;

public record StepCreateAttachmentOperation(int StepNumber, string Name, string Url, long Size);

public record UploadedAttachment(Guid Id, string Name, string Url, long Size);

public record AssignedAttachment(long Id, string Name, string Url, long Size, int Position);