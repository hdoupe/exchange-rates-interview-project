using Microsoft.EntityFrameworkCore;
using ExchangeRatesApi.Models;
using MediatR;
using System.Reflection;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
{
    builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add FluentValidation validators
builder.Services.AddScoped<IValidator<ExchangeRatesApi.Application.CreateExchangeRatesQuery.Command>, ExchangeRatesApi.Application.CreateExchangeRatesQuery.Validator>();

builder.Services.AddDbContext<ExchangeRatesContext>(opt => {
    opt.UseSqlite("ExchangeRatesQueries");
    opt.UseSqlite("CountryCurrencies");
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();
app.UseCors("ApiCorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
