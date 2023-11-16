using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Records;


namespace ExchangeRatesApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesQueryController : ControllerBase
    {
        private static HttpClient treasuryClient = new()
        {
            BaseAddress = new Uri("https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange"),
        };

        private readonly ExchangeRatesContext _context;

        public ExchangeRatesQueryController(ExchangeRatesContext context)
        {
            _context = context;
        }

        // GET: api/ExchangeRatesQuery
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRatesQuery>>> GetExchangeRatesQueries()
        {
          if (_context.ExchangeRatesQueries == null)
          {
              return NotFound();
          }
            return await _context.ExchangeRatesQueries.ToListAsync();
        }

        // Query the fiscal data exchange rates endpoint and return parsed FiscalDataResponse object
        private async Task<FiscalDataResponse> GetFiscalData(ExchangeRatesQuery query) {
            string CountryCurrency = query.CountryCurrency;
            var StartDate = query.StartDate;
            var EndDate = query.EndDate;
            var format = "yyyy-MM-dd";
            var baseQuery = "fields=record_date,country,currency,country_currency_desc,exchange_rate&limit=200";
            var queryString = "";
            if (StartDate != null && EndDate != null) {
                queryString = $"{baseQuery}&filter=country_currency_desc:in:({CountryCurrency}),record_date:gte:{StartDate.Value.ToString(format)},record_date:lte:{EndDate.Value.ToString(format)}";
            } else {
                queryString = $"{baseQuery}&filter=country_currency_desc:in:({CountryCurrency}),record_date:gte:{StartDate.Value.ToString(format)}";
            }
            System.Console.WriteLine($"Querying exchange rates api: {queryString}");
            var fiscalResponse = await treasuryClient.GetAsync($"?{queryString}");
            return await fiscalResponse.Content.ReadFromJsonAsync<FiscalDataResponse>();
        }

        // GET: api/ExchangeRatesQuery/5
        // Retrieves the user's query and runs it against the Fiscal Data exchange rates API.
        [HttpGet("{id}")]
        public async Task<ActionResult<ExchangeRatesResponse>> GetExchangeRatesQuery(long id)
        {
          if (_context.ExchangeRatesQueries == null)
          {
              return NotFound();
          }
            var exchangeRatesQuery = await _context.ExchangeRatesQueries.FindAsync(id);

            if (exchangeRatesQuery == null)
            {
                return NotFound();
            }
            try {
                // Retrieve the data from the fiscal data api endpoint and return query and response.
                var result = await GetFiscalData(exchangeRatesQuery);
                ExchangeRatesResponse response = new(exchangeRatesQuery, result);
                return response;
            } catch (HttpRequestException e) {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.TargetSite);
                return BadRequest(new { message = "Fiscal Data API call was not successful", statusCode = e.StatusCode });
            }
        }

        // PUT: api/ExchangeRatesQuery/5
        // Note: The put endpoint is currently not in use.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExchangeRatesQuery(long id, ExchangeRatesQuery exchangeRatesQuery)
        {
            if (id != exchangeRatesQuery.Id)
            {
                return BadRequest();
            }

            _context.Entry(exchangeRatesQuery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExchangeRatesQueryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExchangeRatesQuery
        // Creates a new exchange rates query. Note this does not retrieve the actual query results since those are
        // retrieved after the user is redirected to the query results page.
        [HttpPost]
        public async Task<ActionResult<ExchangeRatesQuery>> PostExchangeRatesQuery(ExchangeRatesQuery exchangeRatesQuery)
        {
          if (_context.ExchangeRatesQueries == null)
          {
              return Problem("Entity set 'ExchangeRatesContext.ExchangeRatesQueries'  is null.");
          }
            _context.ExchangeRatesQueries.Add(exchangeRatesQuery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExchangeRatesQuery", new { id = exchangeRatesQuery.Id }, exchangeRatesQuery);
        }

        // DELETE: api/ExchangeRatesQuery/5
        // Note: The delete endpoint is currently not in use.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchangeRatesQuery(long id)
        {
            if (_context.ExchangeRatesQueries == null)
            {
                return NotFound();
            }
            var exchangeRatesQuery = await _context.ExchangeRatesQueries.FindAsync(id);
            if (exchangeRatesQuery == null)
            {
                return NotFound();
            }

            _context.ExchangeRatesQueries.Remove(exchangeRatesQuery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExchangeRatesQueryExists(long id)
        {
            return (_context.ExchangeRatesQueries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
