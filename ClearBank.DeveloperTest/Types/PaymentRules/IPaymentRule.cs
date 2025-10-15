namespace ClearBank.DeveloperTest.Types.PaymentRules
{
    public interface IPaymentRule
    {
        bool CanProcess(Account account, MakePaymentRequest request);
    }
}
