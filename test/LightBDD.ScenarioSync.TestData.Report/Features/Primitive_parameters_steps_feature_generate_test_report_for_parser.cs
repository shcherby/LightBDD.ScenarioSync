using LightBDD.Framework;
using LightBDD.Framework.Expectations;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace LightBDD.ScenarioSync.TestData.Report.Features;

[FeatureDescription(
    @"FeatureDescription1
FeatureDescription2 FeatureDescription3
FeatureDescription4 FeatureDescription5")]
[Label("Story-6")]
public class Primitive_parameters_steps_feature_generate_test_report_for_parser : FeatureFixture
{
    [Scenario]
    public void Scenario_with_primitive_steps()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters_expect_to(Expect.To.Not.BeNull<string>(), Expect.To.BeLikeIgnoreCase("100"))
        );
    }

    [Scenario]
    public void Scenario_second_with_primitive_steps()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_without_parameters(),
            c => c.Given_step_with_name_NAME_age_AGE_primitive_parameters("test name", 100)
        );
    }
}