using ExchangeRatesApi.Models;

namespace ExchangeRatesApi.Repositories;

public class CountryRepository : Repository<Country>, ICountryRepository
{
    public CountryRepository(ExchangeRatesContext context) : base(context)
    {
    }
}
