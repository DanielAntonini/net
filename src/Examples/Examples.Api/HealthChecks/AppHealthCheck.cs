using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Examples.Api.HealthChecks;

public class AppHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(
            HealthCheckResult.Healthy("Health Msg."));
    }
}