using MediatR;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;

namespace ExchangeRatesApi.Application;

public class DeleteExchangeRatesQuery
{
    public class Command : IRequest<Unit>
    {
        public long Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ExchangeRatesContext _context;

        public Handler(ExchangeRatesContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (_context.ExchangeRatesQueries == null)
            {
                throw new InvalidOperationException("Entity set 'ExchangeRatesContext.ExchangeRatesQueries' is null.");
            }

            var exchangeRatesQuery = await _context.ExchangeRatesQueries.FindAsync(new object[] { request.Id }, cancellationToken);
            
            if (exchangeRatesQuery == null)
            {
                throw new InvalidOperationException("Exchange rates query not found");
            }

            _context.ExchangeRatesQueries.Remove(exchangeRatesQuery);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
