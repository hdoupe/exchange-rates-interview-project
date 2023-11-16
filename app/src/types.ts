export interface ExchangeRatesQuery {
    id?: number;
    startDate: Date | string;
    endDate?: Date | string | null;
    countryCurrency: string;
    name: string;
}

export interface ExchangeRate {
    record_date: string;
    country: string;
    currency: string;
    country_currency_desc: string;
    exchange_rate: number;
}

export interface QueryResult {
    query: ExchangeRatesQuery;
    response: {
        data: ExchangeRate[];
        meta: { count: number; };
    };
}