# Test Project

## What I have changed

### Included `ConfigDataStoreFactory`.

This is an implementation of `IDataStoreFactory` and can be injected into PaymentService, removing the direct dependency on ConfigurationManager which can be difficult to test.

This also decouples the logic for choosing the correct data store, allowing a separate unit test fixture `ConfigDataStoreFactoryTest` - one less piece of logic to test in `PaymentServiceTest`.

I originally removed the dependency on the appsettings configuration value for determining the data store to use. I decided to put that back, but using `Microsoft.Extensions.Configuration` and mimicked appsettings in `ConfigDataStoreFactoryTest` by passing an in-memory dictionary to the `ConfigDataStoreFactory`.

### Wrote unit tests for `PaymentService` class.

I wrote all the tests before doing any more refactoring on the class, to ensure the behaviour was not altered by the refactoring that followed. 

Over two commits, I think I covered all the code paths.

### Refactored business logic in `PaymentService`.

The `MakePayment` function was not adhering to the single responsibility principle. There was a switch statement with conditional rules depending on the `PaymentScheme`.

I created an interface `IPaymentRule` with three implementations - one for each scheme.

Then I created a `PaymentRuleFactory` (and interface) to expose a `Create(PaymentScheme paymentScheme)` method. 

Once the new factory was leveraged in the `PaymentService` class, the conditional logic was abstracted away, vastly simplifying the class.

### Refactored the `PaymentServiceTest` class.

When I initially wrote the unit tests, I was copy/pasting each test from the last. That left me with a lot of repetition where I instantiated a MakePaymentRequest object and mocked the `IPaymentRuleFactory`.

I created a private method `ArrangePayment` inside the class to take care of instantiating these objects.

## What I would change if I had more time

* I would add summaries above classes, methods, and properties, for conventional inline documentation.
* I would probably rearrange the directory structure to separate model classes from enums - all currently under 'Types'.
* I could add unit test fixtures for each implementation of `IPaymentRule` and `PaymentRuleFactory`.
* I would query what the correct behaviour should be for when `DataStoreType != "Backup"` - at the moment this is what I call a "magic string", and would be better off as an Enum member, perhaps...
