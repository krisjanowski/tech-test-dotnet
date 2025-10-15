namespace ClearBank.DeveloperTest.Types.PaymentRules
{
    public class BacsPaymentRule : IPaymentRule
    {
        public bool CanProcess(Account account, MakePaymentRequest request)
        {
            return account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}
