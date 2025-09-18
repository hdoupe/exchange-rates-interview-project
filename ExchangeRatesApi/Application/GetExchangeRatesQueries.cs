using MediatR;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;

namespace ExchangeRatesApi.Application;

public class GetExchangeRatesQueries
{
    public class Query : IRequest<IEnumerable<ExchangeRatesQuery>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<ExchangeRatesQuery>>
    {
        private readonly ExchangeRatesContext _context;

        public Handler(ExchangeRatesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExchangeRatesQuery>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (_context.ExchangeRatesQueries == null)
            {
                return Enumerable.Empty<ExchangeRatesQuery>();
            }
            
            return await _context.ExchangeRatesQueries.ToListAsync(cancellationToken);
        }
    }
}
