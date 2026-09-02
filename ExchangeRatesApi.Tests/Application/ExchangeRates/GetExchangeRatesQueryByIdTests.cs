using ExchangeRatesApi.Application.ExchangeRates;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Records;
using ExchangeRatesApi.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExchangeRatesApi.Tests.Application.ExchangeRates;

public class GetExchangeRatesQueryByIdTests
{
    private readonly Mock<IExchangeRatesQueryRepository> _mockRepository;
    private readonly GetExchangeRatesQueryById.Handler _handler;

    public GetExchangeRatesQueryByIdTests()
    {
        _mockRepository = new Mock<IExchangeRatesQueryRepository>();
        _handler = new GetExchangeRatesQueryById.Handler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenQueryNotFound_ShouldReturnNull()
    {
        // Arrange
        var request = new GetExchangeRatesQueryById.Query { Id = 999 };
        _mockRepository
            .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRatesQuery?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenQueryFoundWithStartAndEndDate_ShouldReturnExchangeRatesResponse()
    {
        // Arrange
        var queryId = 1L;
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        
        var exchangeRatesQuery = new ExchangeRatesQuery
        {
            Id = queryId,
            Name = "Test Query",
            CountryCurrency = "Canada-Dollar",
            StartDate = startDate,
            EndDate = endDate
        };

        var request = new GetExchangeRatesQueryById.Query { Id = queryId };

        _mockRepository
            .Setup(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRatesQuery);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Query.Should().BeEquivalentTo(exchangeRatesQuery);
        // Verify data came back from the Treasury API 
        result.Response.data.Should().NotBeNull();
        result.Response.meta.Should().NotBeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenQueryFoundWithOnlyStartDate_ShouldReturnExchangeRatesResponse()
    {
        // Arrange
        var queryId = 2L;
        var startDate = new DateTime(2023, 6, 1);
        
        var exchangeRatesQuery = new ExchangeRatesQuery
        {
            Id = queryId,
            Name = "Test Query With Start Date Only",
            CountryCurrency = "United Kingdom-Pound",
            StartDate = startDate,
            EndDate = null
        };

        var request = new GetExchangeRatesQueryById.Query { Id = queryId };

        _mockRepository
            .Setup(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRatesQuery);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Query.Should().BeEquivalentTo(exchangeRatesQuery);
        // Verify data came back from the Treasury API
        result.Response.data.Should().NotBeNull();
        result.Response.meta.Should().NotBeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenQueryFoundWithNoDateRange_ShouldReturnExchangeRatesResponse()
    {
        // Arrange
        var queryId = 3L;
        
        var exchangeRatesQuery = new ExchangeRatesQuery
        {
            Id = queryId,
            Name = "Test Query Without Dates",
            CountryCurrency = "Japan-Yen",
            StartDate = null,
            EndDate = null
        };

        var request = new GetExchangeRatesQueryById.Query { Id = queryId };

        _mockRepository
            .Setup(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRatesQuery);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Query.Should().BeEquivalentTo(exchangeRatesQuery);
        // Verify data came back from the Treasury API
        result.Response.data.Should().NotBeNull();
        result.Response.meta.Should().NotBeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenQueryFoundWithEmptyCountryCurrency_ShouldReturnExchangeRatesResponse()
    {
        // Arrange
        var queryId = 4L;
        
        var exchangeRatesQuery = new ExchangeRatesQuery
        {
            Id = queryId,
            Name = "Test Query Empty Country Currency",
            CountryCurrency = string.Empty,
            StartDate = null,
            EndDate = null
        };

        var request = new GetExchangeRatesQueryById.Query { Id = queryId };

        _mockRepository
            .Setup(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRatesQuery);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Query.Should().BeEquivalentTo(exchangeRatesQuery);
        // Verify data came back from the Treasury API
        result.Response.data.Should().NotBeNull();
        result.Response.meta.Should().NotBeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var queryId = 6L;
        var request = new GetExchangeRatesQueryById.Query { Id = queryId };
        var expectedException = new InvalidOperationException("Database error");

        _mockRepository
            .Setup(r => r.GetByIdAsync(queryId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None));
        
        exception.Message.Should().Be("Database error");
    }
}
