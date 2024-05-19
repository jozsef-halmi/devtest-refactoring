# Taxually technical test - Original description

This solution contains an [API endpoint](https://github.com/Taxually/developer-test/blob/main/Taxually.TechnicalTest/Taxually.TechnicalTest/Controllers/VatRegistrationController.cs) to register a company for a VAT number. Different approaches are required based on the country where the company is based:

- UK companies can register via an API
- French companies must upload a CSV file
- German companies must upload an XML document

The implementation uses the [strategy pattern](https://refactoring.guru/design-patterns/strategy). In order to add support for a new country, the following steps must be done:
1. Create your implementation under `Taxually.TechnicalTest.Application\VatRegistration\Strategies\YourCountryRegistrationStrategy.cs`, which must implement the interface `IVatRegistrationStrategy`. Return the supported country by `.GetSupportedCountry()`.
1. Register the new strategy implementation to the DI container. \Taxually.TechnicalTest.Application\DependencyInjection.cs`
1. Add tests to the `Taxually.TechnicalTest.Application.Tests\VatRegistration` folder.

![Strategy pattern](https://refactoring.guru/images/patterns/diagrams/strategy/solution.png?id=0813a174b29a2ed5902d321aba28e5fc "Strategy Pattern")

Note:
The manual DI registration can be circumvented with using reflection.
```
var strategies = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IVatRegistrationStrategy).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

foreach (var strategy in strategies)
{
    builder.Services.AddTransient(
        typeof(IVatRegistrationStrategy), 
        strategy);
}
```

# Personal notes

## Architecture
For my refactoring exercise, I've used the [clean architecture approach](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture).
Generally speaking, I find this code organization pattern clean, easy to read and easy to test, but I have to admit that for this tiny task this might feel like an overkill.

## Conditional code execution based on country
This can be solved in many ways. I've used the strategy pattern, as I believe it works good in this case, but a lot depends on the (unknown) requirements. 
For example, if it is expected that certain countries will receive a much higher load, the separation can be done on a service level (for example, GermanyRegistration service) which can independently scale. In this way a gateway can route the requests to the corresponding endpoints. This is also a useful approach if there are some strict regulations which strictly define that the API/Data should be hosted within the borders of the company.
It is also worth discussing whether a flow (e.g. filling a form by the customer) should or should not fail in case there is a downtime of the dependant API and whether eventual consistency can be part of the design. (It can also be part of a regulation) If so, an outbox pattern can also be used for messages sent to an internal queue, which then are handled by the corresponding event handlers (invoking a 3rd party web api, putting a message on the queue, etc.)

## Use of async-await
The use of async-await was suboptimal and error-prone throughout the code base, so I've fixed them.

## Configuration
The configuration values were added to the `appsettings.json` which can have separate values per environments.

## Development ideas
- Add proper validation and detach it from the VatRegistration logic (Wrote a TODO to the corresponding code part)
- Error handling
- Structured logging
- Metrics
- Tracing
- Resiliency (retries, outbox pattern)
- Refactor so that API request is independent from the 3rd party UK request (Wrote a TODO to the corresponding code part)
- ...