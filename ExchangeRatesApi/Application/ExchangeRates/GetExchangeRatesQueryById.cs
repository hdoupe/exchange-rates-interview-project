using MediatR;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Records;
using FluentValidation;

namespace ExchangeRatesApi.Application.ExchangeRates;

public class GetExchangeRatesQueryById
{
    public class Query : IRequest<ExchangeRatesResponse?>
    {
        public long Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ExchangeRatesResponse?>
    {
        private readonly ExchangeRatesContext _context;
        private static readonly HttpClient TreasuryClient = new()
        {
            BaseAddress = new Uri("https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange"),
        };

        public Handler(ExchangeRatesContext context)
        {
            _context = context;
        }

        public async Task<ExchangeRatesResponse?> Handle(Query request, CancellationToken cancellationToken)
        {
            if (_context.ExchangeRatesQueries == null)
            {
                return null;
            }

            var exchangeRatesQuery = await _context.ExchangeRatesQueries.FindAsync(new object[] { request.Id }, cancellationToken);

            if (exchangeRatesQuery == null)
            {
                return null;
            }

            var fiscalData = await GetFiscalData(exchangeRatesQuery, cancellationToken);
            return new ExchangeRatesResponse(exchangeRatesQuery, fiscalData);
        }

        private async Task<FiscalDataResponse> GetFiscalData(ExchangeRatesQuery query, CancellationToken cancellationToken)
        {
            string countryCurrency = query.CountryCurrency ?? string.Empty;
            var startDate = query.StartDate;
            var endDate = query.EndDate;
            var format = "yyyy-MM-dd";
            var baseQuery = "fields=record_date,country,currency,country_currency_desc,exchange_rate&limit=200";
            string queryString;
            
            if (startDate != null && endDate != null)
            {
                queryString = $"{baseQuery}&filter=country_currency_desc:in:({countryCurrency}),record_date:gte:{startDate.Value.ToString(format)},record_date:lte:{endDate.Value.ToString(format)}";
            }
            else if (startDate != null)
            {
                queryString = $"{baseQuery}&filter=country_currency_desc:in:({countryCurrency}),record_date:gte:{startDate.Value.ToString(format)}";
            }
            else
            {
                queryString = $"{baseQuery}&filter=country_currency_desc:in:({countryCurrency})";
            }

            System.Console.WriteLine($"Querying exchange rates api: {queryString}");
            var fiscalResponse = await TreasuryClient.GetAsync($"?{queryString}", cancellationToken);
            fiscalResponse.EnsureSuccessStatusCode();
            
            return await fiscalResponse.Content.ReadFromJsonAsync<FiscalDataResponse>(cancellationToken: cancellationToken) 
                ?? new FiscalDataResponse();
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0");
        }
    }
}
