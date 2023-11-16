using ExchangeRatesApi.Models;
using ExchangeRatesApi.Records;

// ExchangeRatesResponse combines the query and fiscal data endpoint response.
public record ExchangeRatesResponse(ExchangeRatesQuery Query, FiscalDataResponse Response);