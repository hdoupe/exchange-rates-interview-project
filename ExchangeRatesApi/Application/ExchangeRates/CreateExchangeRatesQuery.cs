using MediatR;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;
using FluentValidation;

namespace ExchangeRatesApi.Application.ExchangeRates;

public class CreateExchangeRatesQuery
{
    public class Command : IRequest<ExchangeRatesQuery>
    {
        public string? Name { get; set; }
        public string? CountryCurrency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Handler : IRequestHandler<Command, ExchangeRatesQuery>
    {
        private readonly ExchangeRatesContext _context;

        public Handler(ExchangeRatesContext context)
        {
            _context = context;
        }

        public async Task<ExchangeRatesQuery> Handle(Command request, CancellationToken cancellationToken)
        {
            var exchangeRatesQuery = new ExchangeRatesQuery
            {
                Name = request.Name,
                CountryCurrency = request.CountryCurrency,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _context.ExchangeRatesQueries.Add(exchangeRatesQuery);
            await _context.SaveChangesAsync(cancellationToken);

            return exchangeRatesQuery;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.CountryCurrency)
                .NotEmpty()
                .WithMessage("Country Currency is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start Date is required");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End Date must be greater than or equal to Start Date");
        }
    }
}
