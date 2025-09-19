using MediatR;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Repositories;
using FluentValidation;

namespace ExchangeRatesApi.Application.ExchangeRates;

public class GetExchangeRatesQueries
{
    public class Query : IRequest<IEnumerable<ExchangeRatesQuery>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<ExchangeRatesQuery>>
    {
        private readonly IExchangeRatesQueryRepository _repository;

        public Handler(IExchangeRatesQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExchangeRatesQuery>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            // No validation needed for getting all queries
        }
    }
}
