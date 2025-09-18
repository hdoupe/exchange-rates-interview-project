using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExchangeRatesApi.Models;
using ExchangeRatesApi.Application.CountryCurrency;

namespace ExchangeRatesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryCurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryCurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/CountryCurrency
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountryCurrencies()
        {
            var countries = await _mediator.Send(new GetCountryCurrencies.Query());
            return Ok(countries);
        }
    }
}
