using LightBDD.ScenarioSync.Source.Labels;

namespace LightBDD.ScenarioSync.IntegrationTest.Helpers;

public static class MetadataLabelsCreator
{
    public static string CreateAutomatedTestLabel(string testName = "Fake_test_name", string storageName = "FakeStorageName")
        => $"{SyncMetadataLabel.LabelPrefix}{storageName};{testName}";
}