using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TFA.Forum.API.Monitoring;

internal static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddMeter("TFA.Forum.Application")
                .AddPrometheusExporter())
            .WithTracing(builder => builder
                    .ConfigureResource(r => r.AddService("TFA.Forum"))
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter += context =>
                            !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) &&
                            !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
                        options.EnrichWithHttpResponse = (activity, response) =>
                            activity.AddTag("response error", response.StatusCode >= 400);
                    })
                    .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                    .AddJaegerExporter(cfg => cfg.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));

        return services;
    }
}