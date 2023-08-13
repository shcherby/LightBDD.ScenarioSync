using Example.Domain.Domain;
using LightBDD.Framework.Parameters;

namespace LightBDD.ScenarioSync.TestData.Report.Features;

public class Generate_test_report_for_parser_context
{
    public void Given_step_without_parameters()
    {
    }

    public void Given_step_with_name_NAME_age_AGE_primitive_parameters(string name, int age)
    {
    }
    
    public void Given_step_with_name_NAME_age_AGE_primitive_parameters_expect_to(Verifiable<string> name, Verifiable<string> age)
    {
        name.SetActual("not null name");
        age.SetActual("100");
    }

    public void Given_step_with_one_table_parameters(InputTable<Contact> contacts)
    {
    }

    public void Given_step_with_two_table_parameters(InputTable<Contact> contacts, InputTable<Contact> contacts2)
    {
    }

    public void Given_step_with_one_array_tree_parameters(InputTree<ContactAddress[]> contacts)
    {
    }

    public void Given_step_with_one_object_tree_parameters(InputTree<Contact> contact)
    {
    }

    public void Given_step_with_two_object_tree_and_alias_parameters(InputTree<Contact> contact, InputTree<PostalAddress> address, string alias)
    {
    }
}