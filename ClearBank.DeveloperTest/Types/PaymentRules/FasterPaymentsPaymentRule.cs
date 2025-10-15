namespace ClearBank.DeveloperTest.Types.PaymentRules
{
    // Usually I leave in repetitions to stick to a naming convention.
    public class FasterPaymentsPaymentRule : IPaymentRule
    {
        public bool CanProcess(Account account, MakePaymentRequest request)
        {
            return account != null 
                && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
                && account.Balance > request.Amount;
        }
    }
}
