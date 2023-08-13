using LightBDD.Framework.Scenarios;
using LightBDD.ScenarioSync.Extensions.Attributes;
using LightBDD.XUnit2;

namespace LightBDD.ScenarioSync.TestData.Report.Features;

public class Metadata_feature_generate_test_report_for_parser : FeatureFixture
{
    [Scenario]
    [Sync(nameof(Metadata_feature_generate_test_report_for_parser), nameof(Scenario_automated_test_metadata))]
    public void Scenario_automated_test_metadata()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100)
        );
    }
}

public class Metadata_feature_generate_test_report_for_parser2 : FeatureFixture
{
    [Scenario]
    [Sync(nameof(Metadata_feature_generate_test_report_for_parser2), nameof(Scenario_automated_test_metadata2))]
    public void Scenario_automated_test_metadata2()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100)
        );
    }
}

public class Metadata_feature_generate_test_report_for_parser3 : FeatureFixture
{
    [Scenario]
    [Sync(nameof(Metadata_feature_generate_test_report_for_parser3), nameof(Scenario_automated_test_metadata3))]
    public void Scenario_automated_test_metadata3()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100)
        );
    }
}

public class Metadata_feature_4_no_TestSuitePath_attribute : Metadata_feature_5_no_TestSuitePath_attribute
{
    [Scenario]
    [Sync(nameof(Metadata_feature_4_no_TestSuitePath_attribute), nameof(Scenario_automated_test_metadata4))]
    public void Scenario_automated_test_metadata4()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100)
        );
    }
}
public class Metadata_feature_5_no_TestSuitePath_attribute : FeatureFixture
{
    [Scenario]
    [Sync(nameof(Metadata_feature_4_no_TestSuitePath_attribute), nameof(Scenario_automated_test_metadata5))]
    public void Scenario_automated_test_metadata5()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100)
        );
    }
}
