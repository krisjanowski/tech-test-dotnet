namespace ClearBank.DeveloperTest.Types.PaymentRules
{
    public class ChapsPaymentRule : IPaymentRule
    {
        public bool CanProcess(Account account, MakePaymentRequest request)
        {
            return account != null 
                && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)
                && account.Status == AccountStatus.Live;
        }
    }
}
