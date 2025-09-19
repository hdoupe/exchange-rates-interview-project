using ExchangeRatesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesApi.Repositories;

public class ExchangeRatesQueryRepository : Repository<ExchangeRatesQuery>, IExchangeRatesQueryRepository
{
    public ExchangeRatesQueryRepository(ExchangeRatesContext context) : base(context)
    {
    }

    public override async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }
}
