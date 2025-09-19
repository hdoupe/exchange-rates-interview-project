using MediatR;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Repositories;
using FluentValidation;

namespace ExchangeRatesApi.Application.CountryCurrency;

public class GetCountryCurrencies
{
    public class Query : IRequest<IEnumerable<Country>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<Country>>
    {
        private readonly ICountryRepository _repository;

        public Handler(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Country>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
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
