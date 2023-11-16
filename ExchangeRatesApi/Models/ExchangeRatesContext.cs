using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesApi.Models;

// ExchangeRatesContext connects the models with the local Sqlite database.
public class ExchangeRatesContext : DbContext
{
    public string DbPath { get; }

    public ExchangeRatesContext(DbContextOptions<ExchangeRatesContext> options)
        : base(options)
    {
        DbPath = @"db/exchange_rates.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    public DbSet<ExchangeRatesQuery> ExchangeRatesQueries { get; set; } = null!;
    public DbSet<Country> CountryCurrencies { get; set; } = null!;

}