using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;

namespace ExchangeRatesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryCurrencyController : ControllerBase
    {
        private readonly ExchangeRatesContext _context;

        public CountryCurrencyController(ExchangeRatesContext context)
        {
            _context = context;
        }

        // GET: api/CountryCurrency
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountryCurrencies()
        {
          if (_context.CountryCurrencies == null)
          {
              return NotFound();
          }
            return await _context.CountryCurrencies.ToListAsync();
        }
    }
}
