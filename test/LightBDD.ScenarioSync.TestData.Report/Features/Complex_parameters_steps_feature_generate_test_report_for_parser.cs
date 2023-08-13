using Example.Domain.Domain;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace LightBDD.ScenarioSync.TestData.Report.Features;

public class Complex_parameters_steps_feature_generate_test_report_for_parser : FeatureFixture
{
    [Scenario]
    public void Scenario_with_table_parameters_steps()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_with_one_table_parameters(
                Table.For(
                    new Contact("John", "111-222-333", "john123@gmail.com"),
                    new Contact("John", "111-303-404", "jo@hotmail.com"))),
            c => c.Given_step_with_two_table_parameters(
                Table.For(
                    new Contact("John", "111-222-333", "john123@gmail.com"),
                    new Contact("John", "111-303-404", "jo@hotmail.com")),
                Table.For(
                    new Contact("John", "111-222-333", "john123@gmail.com"),
                    new Contact("John", "111-303-404", "jo@hotmail.com")))
            );
    }

    [Scenario]
    public void Scenario_with_tree_parameters_steps()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_with_one_array_tree_parameters(new[]
            {
                new ContactAddress("Joey",
                    new Contact("Joe Jonnes", "666777888", "joe67@email.com"),
                    new PostalAddress("UK", "London", "AB1 2CD", "47 Main Street")),
                new ContactAddress("Janek",
                    new Contact("Jan Nowak", "123654789", "nowak33@email.com"),
                    new PostalAddress("Poland", "Kraków", "31-042", "Rynek Główny 1"))
            }),
             c => c.Given_step_with_one_object_tree_parameters(
                new Contact("Joe Jonnes", "666777888", "joe67@email.com")),
             c => c.Given_step_with_two_object_tree_and_alias_parameters(
                new Contact("Joe Jonnes", "666777888", "joe67@email.com"),
                new PostalAddress("UK", "London", "AB1 2CD", "47 Main Street"),
                "alias")
            );
    }

    [Scenario]
    public void Scenario_with_table_and_tree_parameters_steps()
    {
        Runner.WithContext<Generate_test_report_for_parser_context>().RunScenario(
            c => c.Given_step_with_one_table_parameters(
                Table.For(
                    new Contact("John", "111-222-333", "john123@gmail.com"),
                    new Contact("John", "111-303-404", "jo@hotmail.com"))),
             c => c.Given_step_with_one_object_tree_parameters(
                new Contact("Joe Jonnes", "666777888", "joe67@email.com"))
            );
    }
}