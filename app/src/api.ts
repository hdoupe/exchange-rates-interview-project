import axios, { AxiosResponse } from "axios"
import { ExchangeRatesQuery, QueryResult } from "./types";

console.log('BASE', process.env.REACT_APP_BASE_URL);

const client = axios.create({
    baseURL: process.env.REACT_APP_BASE_URL,
    timeout: 5000,
});

export const ExchangeRates = {
    list: async (): Promise<AxiosResponse<ExchangeRatesQuery[]>> => {
        return await client.get('/api/ExchangeRatesQuery/')
    },

    get: async (id: number): Promise<AxiosResponse<QueryResult>> => {
        return await client.get(`/api/ExchangeRatesQuery/${id}`)
    },

    post: async (data: ExchangeRatesQuery): Promise<AxiosResponse<ExchangeRatesQuery>> => {
        return await client.post('/api/ExchangeRatesQuery', data);
    },

    put: async (id: number, data: ExchangeRatesQuery): Promise<AxiosResponse<ExchangeRatesQuery>> => {
        return await client.post(`/api/ExchangeRatesQuery/${id}`, data);
    }
}

export const CountryCurrencies = {
    list: async (): Promise<AxiosResponse<{ id: number; name: string; }[]>> => {
        return await client.get('/api/CountryCurrency')
    },
}

const Api = { ExchangeRates, CountryCurrencies };

export default Api;