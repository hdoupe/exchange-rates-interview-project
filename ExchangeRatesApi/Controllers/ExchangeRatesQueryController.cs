using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Records;
using ExchangeRatesApi.Application;


namespace ExchangeRatesApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesQueryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRatesQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/ExchangeRatesQuery
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRatesQuery>>> GetExchangeRatesQueries()
        {
            var queries = await _mediator.Send(new GetExchangeRatesQueries.Query());
            return Ok(queries);
        }

        // GET: api/ExchangeRatesQuery/5
        // Retrieves the user's query and runs it against the Fiscal Data exchange rates API.
        [HttpGet("{id}")]
        public async Task<ActionResult<ExchangeRatesResponse>> GetExchangeRatesQuery(long id)
        {
            try
            {
                var result = await _mediator.Send(new GetExchangeRatesQueryById.Query { Id = id });
                
                if (result == null)
                {
                    return NotFound();
                }
                
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.TargetSite);
                return BadRequest(new { message = "Fiscal Data API call was not successful", statusCode = e.Data["StatusCode"] });
            }
        }

        // PUT: api/ExchangeRatesQuery/5
        // Note: The put endpoint is currently not in use.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExchangeRatesQuery(long id, UpdateExchangeRatesQuery.Command command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        // POST: api/ExchangeRatesQuery
        // Creates a new exchange rates query. Note this does not retrieve the actual query results since those are
        // retrieved after the user is redirected to the query results page.
        [HttpPost]
        public async Task<ActionResult<ExchangeRatesQuery>> PostExchangeRatesQuery(CreateExchangeRatesQuery.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetExchangeRatesQuery", new { id = result.Id }, result);
        }

        // DELETE: api/ExchangeRatesQuery/5
        // Note: The delete endpoint is currently not in use.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchangeRatesQuery(long id)
        {
            try
            {
                await _mediator.Send(new DeleteExchangeRatesQuery.Command { Id = id });
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
