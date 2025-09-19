using MediatR;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;
using FluentValidation;

namespace ExchangeRatesApi.Application.ExchangeRates;

public class UpdateExchangeRatesQuery
{
    public class Command : IRequest<Unit>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? CountryCurrency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
            var exchangeRatesQuery = new ExchangeRatesQuery
            {
                Id = request.Id,
                Name = request.Name,
                CountryCurrency = request.CountryCurrency,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _context.Entry(exchangeRatesQuery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExchangeRatesQueryExists(request.Id, cancellationToken))
                {
                    throw new InvalidOperationException("Exchange rates query not found");
                }
                else
                {
                    throw;
                }
            }

            return Unit.Value;
        }

        private async Task<bool> ExchangeRatesQueryExists(long id, CancellationToken cancellationToken)
        {
            return await _context.ExchangeRatesQueries.AnyAsync(e => e.Id == id, cancellationToken);
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0");

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
