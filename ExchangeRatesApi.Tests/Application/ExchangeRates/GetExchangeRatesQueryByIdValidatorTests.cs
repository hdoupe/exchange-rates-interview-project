using ExchangeRatesApi.Application.ExchangeRates;
using FluentAssertions;
using Xunit;

namespace ExchangeRatesApi.Tests.Application.ExchangeRates;

public class GetExchangeRatesQueryByIdValidatorTests
{
    private readonly GetExchangeRatesQueryById.Validator _validator;

    public GetExchangeRatesQueryByIdValidatorTests()
    {
        _validator = new GetExchangeRatesQueryById.Validator();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(long.MaxValue)]
    public void Validate_WhenIdIsGreaterThanZero_ShouldBeValid(long id)
    {
        // Arrange
        var query = new GetExchangeRatesQueryById.Query { Id = id };

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(long.MinValue)]
    public void Validate_WhenIdIsZeroOrNegative_ShouldBeInvalid(long id)
    {
        // Arrange
        var query = new GetExchangeRatesQueryById.Query { Id = id };

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].PropertyName.Should().Be("Id");
        result.Errors[0].ErrorMessage.Should().Be("Id must be greater than 0");
    }
}
