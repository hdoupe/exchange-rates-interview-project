using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesApi.Records;

// Records for serializing and desrializing data from the fiscaldata API endpoint.
public record FiscalDataResponse {
    public List<ExchangeRate>? data { get; set; }
    public Meta? meta { get; set; }
    public Links? links { get; set; }
}

public record ExchangeRate {
    [DataType(DataType.Date)]
    public DateTime? record_date { get; set; }
    public string? country { get; set; }
    public string? currency { get; set; }
    public string? country_currency_desc { get; set; }
    public double? exchange_rate { get; set; }
};

public record Meta {
    public int count { get; set; }
}

public record Links {
    public string? next { get; set; }
}