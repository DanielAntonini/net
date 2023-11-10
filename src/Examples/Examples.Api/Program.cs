using Example.Infrastructure.Configuration;
using Examples.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configurationManager = builder.Configuration;

// Read AppSettings
var configuration = configurationManager.GetSection("Configuration").Get<Configuration>();
var configurationValue = configurationManager.GetValue<int>("Configuration:Value");

// Services
builder.Services.AddTransient<ExceptionHandlerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use Exception Handler
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();