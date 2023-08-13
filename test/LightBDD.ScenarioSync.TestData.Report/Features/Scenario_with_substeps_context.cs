using System.Text;
using Example.Domain.Helpers;
using LightBDD.Framework;
using LightBDD.Framework.Reporting;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace LightBDD.ScenarioSync.TestData.Report.Features;

public class Scenario_with_substeps_context
{
    public async Task<CompositeStep> When_customer_adds_products_to_basket()
    {
        return CompositeStep.DefineNew()
            .AddAsyncSteps(
                _ => Given_product_is_in_stock("wooden desk"),
                _ => When_customer_adds_product_to_the_basket("wooden desk"),
                _ => Then_the_product_addition_should_be_successful())
            .Build();
    }

    public async Task<CompositeStep> Then_customer_should_receive_order_email()
    {
        return CompositeStep.DefineNew()
            .WithContext<MailBox>()
            .AddAsyncSteps(
                x => x.Then_customer_should_receive_invoice(),
                x => x.Then_customer_should_receive_order_confirmation())
            .Build();
    }

    public async Task Given_product_is_in_stock(string product)
    {
    }

    public async Task When_customer_adds_product_to_the_basket(string product)
    {
    }

    public async Task Then_the_product_addition_should_be_successful()
    {
    }

    public async Task<CompositeStep> Given_customer_is_logged_in()
    {
        return CompositeStep.DefineNew()
            .AddSteps(
                Given_the_user_is_about_to_login,
                Given_the_user_entered_valid_login,
                Given_the_user_entered_valid_password,
                When_the_user_clicks_login_button,
                Then_the_login_operation_should_be_successful)
            .Build();
    }

    public async Task<CompositeStep> When_customer_pays_for_products_in_basket()
    {
        return CompositeStep.DefineNew()
            .AddSteps(
                When_customer_requests_to_pay,
                Then_payment_should_be_successful)
            .Build();
    }

    private void Then_payment_should_be_successful()
    {
    }

    private void When_customer_requests_to_pay()
    {
        LongRunningOperationSimulator.Simulate();
    }

    #region Login steps

    private void Then_the_login_operation_should_be_successful()
    {
    }

    private void When_the_user_clicks_login_button()
    {
        LongRunningOperationSimulator.Simulate();
    }

    private void Given_the_user_entered_valid_password()
    {
    }

    private void Given_the_user_entered_valid_login()
    {
    }

    private void Given_the_user_is_about_to_login()
    {
    }

    #endregion
    
    private class MailBox
    {
        public async Task Then_customer_should_receive_invoice()
        {
            await StepExecution.Current.AttachFile(m => m.CreateFromText("invoice-content", "txt", "Example invoice content", Encoding.UTF8));
            StepExecution.Current.IgnoreScenario("Not implemented yet");
        }

        public async Task Then_customer_should_receive_order_confirmation()
        {
        }
    }
}