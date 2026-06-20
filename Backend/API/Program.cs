using API.Extensions;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using Persistence.Extensions;
using Application.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services
    .AddFluentValidationAutoValidation();

// Clean registration - Extension registration
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddApplicationSettings(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddJwtAuthentication();
builder.Services.AddRateLimiting();
builder.Services.AddValidators();
builder.Services.AddSwaggerServices();

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerMiddleware();
}

app.UseHttpsRedirection();
app.UseApplicationMiddleware();
app.UseResponseCompression();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

app.Run();
