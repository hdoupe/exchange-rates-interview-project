using MediatR;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Repositories;
using FluentValidation;

namespace ExchangeRatesApi.Application.ExchangeRates;

public class DeleteExchangeRatesQuery
{
    public class Command : IRequest<Unit>
    {
        public long Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IExchangeRatesQueryRepository _repository;

        public Handler(IExchangeRatesQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var exchangeRatesQuery = await _repository.GetByIdAsync(request.Id, cancellationToken);
            
            if (exchangeRatesQuery == null)
            {
                throw new InvalidOperationException("Exchange rates query not found");
            }

            await _repository.DeleteAsync(exchangeRatesQuery, cancellationToken);

            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0");
        }
    }
}
