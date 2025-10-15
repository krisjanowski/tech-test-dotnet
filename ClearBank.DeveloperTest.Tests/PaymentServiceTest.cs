using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Factory;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Types.PaymentRules;
using Moq;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class PaymentServiceTest
    {
        private Mock<IAccountDataStore> MockDataStore;
        private Mock<IDataStoreFactory> MockDataStoreFactory;
        private Mock<IPaymentRuleFactory> MockPaymentRuleFactory;
        private PaymentService PaymentService;

        private MakePaymentRequest ArrangePayment(PaymentScheme scheme)
        {
            var request = new MakePaymentRequest
            {
                CreditorAccountNumber = "1234",
                DebtorAccountNumber = "4321",
                PaymentDate = DateTime.UtcNow,
                Amount = 99,
                PaymentScheme = scheme
            };
           
            switch (scheme)
            {
                case PaymentScheme.Bacs:
                    MockPaymentRuleFactory.Setup(f => f.Create(PaymentScheme.Bacs)).Returns(new BacsPaymentRule());
                    break;
                case PaymentScheme.FasterPayments:
                    MockPaymentRuleFactory.Setup(f => f.Create(PaymentScheme.FasterPayments)).Returns(new FasterPaymentsPaymentRule());
                    break;
                case PaymentScheme.Chaps:
                    MockPaymentRuleFactory.Setup(f => f.Create(PaymentScheme.Chaps)).Returns(new ChapsPaymentRule());
                    break;
            }

            return request;
        }

        [SetUp]
        public void Setup()
        {
            MockDataStore = new Mock<IAccountDataStore>();

            MockDataStoreFactory = new Mock<IDataStoreFactory>();
            MockDataStoreFactory.Setup(x => x.Create()).Returns(MockDataStore.Object);

            MockPaymentRuleFactory = new Mock<IPaymentRuleFactory>();

            PaymentService = new PaymentService(MockDataStoreFactory.Object, MockPaymentRuleFactory.Object);
        }

        [Test]
        public void TestMakePaymentWithFasterPayments_AccountCannotBeNull()
        {
            var request = ArrangePayment(PaymentScheme.FasterPayments);

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithBacs_AccountCannotBeNull()
        {
            var request = ArrangePayment(PaymentScheme.Bacs);

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithChaps_AccountCannotBeNull()
        {
            var request = ArrangePayment(PaymentScheme.Chaps);

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithFasterPayments_AllowedPaymentSchemesDoesNotIncludeFlag()
        {
            var request = ArrangePayment(PaymentScheme.FasterPayments);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, // Does not have FasterPayments flag
                Status = AccountStatus.Live
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithBacs_AllowedPaymentSchemesDoesNotIncludeFlag()
        {
            var request = ArrangePayment(PaymentScheme.Bacs);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, // Does not have Bacs flag
                Status = AccountStatus.Live
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithChaps_AllowedPaymentSchemesDoesNotIncludeFlag()
        {
            var request = ArrangePayment(PaymentScheme.Chaps);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, // Does not have Chaps flag
                Status = AccountStatus.Live
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithFasterPayments_AllowedPaymentSchemesHasFlag_InsufficientBalance()
        {
            var request = ArrangePayment(PaymentScheme.FasterPayments);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 10, // Balance less than Amount
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, // Correct Flag
                Status = AccountStatus.Disabled
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithFasterPayments_AllowedPaymentSchemesHasFlag_AdequateBalance()
        {
            var request = ArrangePayment(PaymentScheme.FasterPayments);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100, // Balance greater than Amount
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, // Correct Flag
                Status = AccountStatus.Disabled
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            MockDataStore.Verify(x => x.UpdateAccount(It.Is<Account>(a => a.AccountNumber == "4321" && a.Balance == 1)), Times.Once);
        }

        [Test]
        public void TestMakePaymentWithBacs_AllowedPaymentSchemesHasFlag()
        {
            var request = ArrangePayment(PaymentScheme.Bacs);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, // Correct Flag
                Status = AccountStatus.Live
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            MockDataStore.Verify(x => x.UpdateAccount(It.Is<Account>(a => a.AccountNumber == "4321" && a.Balance == 1)), Times.Once);
        }

        [Test]
        public void TestMakePaymentWithChaps_AllowedPaymentSchemesHasFlag_StatusDisabled()
        {
            var request = ArrangePayment(PaymentScheme.Chaps);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, // Correct Flag
                Status = AccountStatus.Disabled // Disabled Status
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);

            MockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void TestMakePaymentWithChaps_AllowedPaymentSchemesHasFlag_StatusLive()
        {
            var request = ArrangePayment(PaymentScheme.Chaps);

            MockDataStore.Setup(x => x.GetAccount("4321")).Returns(new Account()
            {
                AccountNumber = "4321",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, // Correct Flag
                Status = AccountStatus.Live // Live Status
            });

            var result = PaymentService.MakePayment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            MockDataStore.Verify(x => x.UpdateAccount(It.Is<Account>(a => a.AccountNumber == "4321" && a.Balance == 1)), Times.Once);
        }
    }
}
