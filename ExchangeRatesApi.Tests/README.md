# ExchangeRatesApi.Tests

This test project contains unit tests for the ExchangeRatesApi application.

## Test Framework

- **xUnit** - Testing framework
- **Moq** - Mocking library for creating test doubles
- **FluentAssertions** - Fluent assertion library for more readable tests

## Test Coverage

### GetExchangeRatesQueryById Tests

The `GetExchangeRatesQueryByIdTests` class contains comprehensive unit tests for the `GetExchangeRatesQueryById.Handler`:

1. **Handle_WhenQueryNotFound_ShouldReturnNull** - Verifies that the handler returns null when a query is not found in the repository.

2. **Handle_WhenQueryFoundWithStartAndEndDate_ShouldReturnExchangeRatesResponse** - Tests the handler with a query that has both start and end dates.

3. **Handle_WhenQueryFoundWithOnlyStartDate_ShouldReturnExchangeRatesResponse** - Tests the handler with a query that only has a start date.

4. **Handle_WhenQueryFoundWithNoDateRange_ShouldReturnExchangeRatesResponse** - Tests the handler when no date range is specified.

5. **Handle_WhenQueryFoundWithEmptyCountryCurrency_ShouldReturnExchangeRatesResponse** - Tests the handler with an empty country currency string.

6. **Handle_WhenCancellationRequested_ShouldPassCancellationToken** - Verifies that cancellation tokens are properly passed through to the repository.

7. **Handle_WhenRepositoryThrowsException_ShouldPropagateException** - Tests exception handling and propagation.

### GetExchangeRatesQueryById Validator Tests

The `GetExchangeRatesQueryByIdValidatorTests` class tests the FluentValidation validator:

1. **Validate_WhenIdIsGreaterThanZero_ShouldBeValid** - Tests that valid IDs (> 0) pass validation.

2. **Validate_WhenIdIsZeroOrNegative_ShouldBeInvalid** - Tests that invalid IDs (≤ 0) fail validation with the appropriate error message.

## Running Tests

To run all tests:

```bash
dotnet test
```

To run tests with detailed output:

```bash
dotnet test --verbosity detailed
```

To run tests and generate code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Structure

Tests follow the AAA (Arrange-Act-Assert) pattern:

- **Arrange**: Set up test data and mock dependencies
- **Act**: Execute the method being tested
- **Assert**: Verify the expected outcome using FluentAssertions

## Mocking Strategy

The tests use Moq to mock the `IExchangeRatesQueryRepository` interface, allowing us to test the handler in isolation without requiring a real database connection.

## Notes

- The tests make actual HTTP calls to the Treasury API. In a production environment, you may want to mock the HttpClient as well using libraries like `MockHttp` or `WireMock.Net`.
- Tests verify both successful paths and error conditions.
- FluentAssertions provides readable assertion syntax that makes test failures easier to understand.
