using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace LightBDD.ScenarioSync.TestData.Report.Features;

public class Scenarios_with_substeps : FeatureFixture
{
    [Scenario]
    public async Task Scenario_with_one_level_sub_steps()
    {
        await Runner.WithContext<Scenario_with_substeps_context>().RunScenarioAsync(
            c => c.Given_customer_is_logged_in(),
            c => c.When_customer_adds_products_to_basket(),
            c => c.When_customer_pays_for_products_in_basket(),
            c => c.Then_customer_should_receive_order_email()
        );
    }
    
    [Scenario]
    public async Task Scenario_with_two_level_sub_steps()
    {
        await Runner.WithContext<Scenario_with_substeps_context>().RunScenarioAsync(
            c => c.Given_customer_is_logged_in(),
            c => c.When_customer_adds_products_to_basket(),
            c => c.When_customer_pays_for_products_in_basket(),
            c => c.Then_customer_should_receive_order_email()
        );
    }
}