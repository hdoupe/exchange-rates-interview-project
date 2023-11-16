# Exchange Rates API

## Run the API Server

### Visual Studio

Load the solution `ExchangeRatesApi.sln` in VisualStudio and click run.

### Or, you can use the `dotnet` CLI tool:

Run the server in development mode via: `dotnet watch`

Run the server in production mode via: `dotnet run`

API Documentation is available here: http://localhost:8000/swagger/index.html

## Database Set up

This API uses a Sqlite database, but it could be configured to use a more production ready database like Postgres or MySQL.

The database is located here [db/exchange_rates.db](db/exchange_rates.db) and is ready to be used. To re-create it, remove the existing file and re-run the migrations using the following commands:

```
# Optionally remove local database before settting up a new one:
rm db/exchange_rates.db
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef database update
```

## Extra Resources

Here are some resources that were helpful for implementing the API:
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-code
- https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model?view=aspnetcore-7.0
- https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
- https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
