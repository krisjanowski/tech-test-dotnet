using ClearBank.DeveloperTest.Factory;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataStoreFactory DataStoreFactory;
        private readonly IPaymentRuleFactory PaymentRuleFactory;

        public PaymentService(IDataStoreFactory dataStoreFactory, IPaymentRuleFactory paymentRuleFactory)
        {
            DataStoreFactory = dataStoreFactory;
            PaymentRuleFactory = paymentRuleFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStore = DataStoreFactory.Create();

            var account = dataStore.GetAccount(request.DebtorAccountNumber);

            var paymentRule = PaymentRuleFactory.Create(request.PaymentScheme);

            var canProcess = paymentRule.CanProcess(account, request);

            if (canProcess)
            {
                account.Balance -= request.Amount;

                dataStore.UpdateAccount(account);
            }

            return new MakePaymentResult(canProcess);
        }
    }
}
