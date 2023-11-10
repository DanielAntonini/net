using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Example.Infrastructure.Configuration;
using Examples.Api.HealthChecks;
using Examples.Api.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;

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

// IMemoryCache
builder.Services.AddMemoryCache();

// Feature Management
builder.Services.AddFeatureManagement();

// Custom HealthCheck
builder.Services.AddHealthChecks().AddCheck<AppHealthCheck>("App", tags: new [] { "app" });
builder.Services.AddHealthChecks().AddCheck<DataBaseHealthCheck>("DataBase", tags: new []{ "database"});


// Services
builder.Services.AddTransient<ExceptionHandlerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// HealthCheck - Configure Endpoint to expose the HealthCheck Info
app.MapHealthChecks("/_health",new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = registration => registration.Tags.Contains("app"),
    ResponseWriter = (context, report) =>
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string json = JsonSerializer.Serialize(
            new
            {
                Status = report.Status.ToString(),
                Duration = report.TotalDuration,
                Info = report.Entries
                    .Select(e =>
                        new
                        {
                            Key = e.Key,
                            Description = e.Value.Description,
                            Duration = e.Value.Duration,
                            Status = Enum.GetName(
                                typeof(HealthStatus),
                                e.Value.Status),
                            Error = e.Value.Exception?.Message,
                            Data = e.Value.Data
                        })
                    .ToList()
            },
            jsonSerializerOptions);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsync(json); 
    }
});

app.MapHealthChecks("/_health2",new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = registration => registration.Tags.Contains("database")
});

app.UseHttpsRedirection();

app.UseAuthorization();

// Use Exception Handler
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();