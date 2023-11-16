using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesApi.Models;

// ExchangeRatesQuery stores the parameters for looking up exchange rate values.
// The exchange rates are retrieved dynamically and are not stored in our database.
public class ExchangeRatesQuery {
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? CountryCurrency { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? EndDate { get; set; }
}