using Demo.LightBDD.XUnit2.Core.Attributes;
using Demo.LightBDD.XUnit2.Domain;
using Example.Domain.Domain;
using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace Demo.LightBDD.XUnit2.Features
{
    [FeatureDescription(
        @"In order to maintain my contact book
As an application user
I want to add, browse and remove my contacts")]
    [Relations(123)]
    public class Contacts_management : FeatureFixture
    {
        [Scenario]
        [Sync(nameof(Contacts_management), nameof(Contact_book_should_allow_me_to_add_multiple_contacts))]
        public void Contact_book_should_allow_me_to_add_multiple_contacts()
        {
            Runner.WithContext<ContactsManagementContext>().RunScenario(
                _ => _.Given_my_contact_book_is_empty(),
                _ => _.When_I_add_new_contacts(),
                _ => _.Then_all_contacts_should_be_available_in_the_contact_book());
        }

        [Scenario]
        [Sync(nameof(Contacts_management), nameof(Contact_book_should_allow_me_to_remove_contacts))]
        public void Contact_book_should_allow_me_to_remove_contacts()
        {
            Runner.WithContext<ContactsManagementContext>().RunScenario(
                _ => _.Given_my_contact_book_is_filled_with_contacts(),
                _ => _.When_I_remove_one_contact(),
                _ => _.Then_the_contact_book_should_not_contain_removed_contact_any_more(),
                _ => _.Then_the_contact_book_should_contains_all_other_contacts());
        }

        [Scenario]
        [Sync(nameof(Contacts_management), nameof(Contact_book_should_allow_me_to_remove_all_contacts))]
        public void Contact_book_should_allow_me_to_remove_all_contacts()
        {
            Runner.WithContext<ContactsManagementContext>().RunScenario(
                c => c.Given_my_contact_book_is_filled_with_many_contacts(),
                c => c.When_I_clear_it(),
                c => c.Then_the_contact_book_should_be_empty());
        }

        [Scenario]
        [Label("Test-label")]
        [Relations(234, 235)]
        [Sync(nameof(Contacts_management), nameof(Searching_for_contacts_by_phone))]
        public void Searching_for_contacts_by_phone()
        {
            Runner.WithContext<ContactsManagementContext>().RunScenario(
                c => c.Given_my_contact_book_is_empty(),
                c => c.Given_I_added_contacts(Table.For(
                    new Contact("John", "111-222-333", "john@hotmail.com"),
                    new Contact("John", "111-303-404", "jo@hotmail.com"),
                    new Contact("Greg", "213-444-444", "greg22@gmail.com"),
                    new Contact("Emily", "111-222-5556", "emily1@gmail.com"),
                    new Contact("Kathy", "111-555-330", "ka321@gmail.com"))),
                c => c.When_I_search_for_contacts_by_phone_starting_with("111"),
                c => c.Then_I_should_receive_contacts(Table.ExpectData(
                    b => b.WithInferredColumns()
                        .WithKey(x => x.Name),
                    new Contact("Emily", "111-222-5556", "emily1@gmail.com"),
                    new Contact("John", "111-222-333", "john@hotmail.com"),
                    new Contact("John", "111-303-404", "jo@hotmail.com"),
                    new Contact("Kathy", "111-555-330", "ka321@gmail.com")
                )));
        }

        [Scenario]
        [Sync(nameof(Contacts_management), nameof(Displaying_contacts_alphabetically))]
        public void Displaying_contacts_alphabetically()
        {
            Runner.WithContext<ContactsManagementContext>().RunScenario(
                c => c.Given_my_contact_book_is_empty(),
                c => c.Given_I_added_contacts(Table.For(
                    new Contact("John", "111-222-333", "john123@gmail.com"),
                    new Contact("Greg", "213-444-444", "greg22@gmail.com"),
                    new Contact("Emily", "111-222-5556", "emily1@gmail.com"),
                    new Contact("Kathy", "111-555-330", "ka321@gmail.com"))),
                c => c.When_I_request_contacts_sorted_by_name(),
                c => c.Then_I_should_receive_contacts(Table.ExpectData(
                    new Contact("Emily", "111-222-5556", "emily1@gmail.com"),
                    new Contact("Greg", "213-444-444", "greg22@gmail.com"),
                    new Contact("John", "111-222-333", "john123@gmail.com"),
                    new Contact("Kathy", "111-555-330", "ka321@gmail.com"))));
        }
    }
}