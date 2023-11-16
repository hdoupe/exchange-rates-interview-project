namespace ExchangeRatesApi.Models;

// Country model stores the country-currency values that are recognized by the API.
public class Country
{
    public long Id { get; set; }
    public string? Name { get; set; }
}