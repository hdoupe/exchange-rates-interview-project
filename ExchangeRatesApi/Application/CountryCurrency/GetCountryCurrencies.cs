using MediatR;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;
using FluentValidation;

namespace ExchangeRatesApi.Application.CountryCurrency;

public class GetCountryCurrencies
{
    public class Query : IRequest<IEnumerable<Country>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<Country>>
    {
        private readonly ExchangeRatesContext _context;

        public Handler(ExchangeRatesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (_context.CountryCurrencies == null)
            {
                return Enumerable.Empty<Country>();
            }

            return await _context.CountryCurrencies.ToListAsync(cancellationToken);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            // No validation needed for getting all country currencies
        }
    }
}
