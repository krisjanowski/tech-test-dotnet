using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.PaymentRules;
namespace ClearBank.DeveloperTest.Factory
{
    public interface IPaymentRuleFactory
    {
        IPaymentRule Create(PaymentScheme paymentScheme);
    }
}
