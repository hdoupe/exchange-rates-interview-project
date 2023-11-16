using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

#nullable disable

namespace ExchangeRatesApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedCountryCurrencyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedData = new List<string>();
            using (StreamReader r = new StreamReader(@"Migrations/country_currency.json"))
            {
                string json = r.ReadToEnd();
                seedData = JsonConvert.DeserializeObject<List<string>>(json);
            }
            seedData.ForEach(country => {
                migrationBuilder.Sql($"INSERT INTO CountryCurrencies (Name) VALUES (\"{country}\");", false);
            });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from CountryCurrencies;", false);
        }
    }
}
