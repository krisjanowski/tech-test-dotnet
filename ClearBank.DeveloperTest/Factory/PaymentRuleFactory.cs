using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.PaymentRules;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Factory
{
    public class PaymentRuleFactory : IPaymentRuleFactory
    {
        private readonly Dictionary<PaymentScheme, IPaymentRule> PaymentRules;

        public PaymentRuleFactory()
        {
            PaymentRules = new Dictionary<PaymentScheme, IPaymentRule>
            {
                { PaymentScheme.Bacs, new BacsPaymentRule() },
                { PaymentScheme.FasterPayments, new FasterPaymentsPaymentRule() },
                { PaymentScheme.Chaps, new ChapsPaymentRule() }
            };
        }

        public IPaymentRule Create(PaymentScheme paymentScheme)
        {
            if (PaymentRules.TryGetValue(paymentScheme, out var paymentRule))
            {
                return paymentRule;
            }

            return null;
        }
    }
}
