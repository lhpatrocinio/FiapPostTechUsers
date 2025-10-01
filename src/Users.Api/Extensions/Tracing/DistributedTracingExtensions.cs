using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Users.Api.Extensions.Tracing
{
    /// <summary>
    /// Extensões para configuração de Distributed Tracing com OpenTelemetry
    /// Implementa observabilidade completa seguindo padrões FIAP Phase 3
    /// </summary>
    public static class DistributedTracingExtensions
    {
        private static readonly ActivitySource ActivitySource = new("FiapPosTech.Users");

        /// <summary>
        /// Configura OpenTelemetry Distributed Tracing para Users API
        /// </summary>
        public static IServiceCollection AddDistributedTracing(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceName = "fiap-users-api";
            var serviceVersion = "1.0.0";

            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService(serviceName, serviceVersion)
                            .AddAttributes(new Dictionary<string, object>
                            {
                                ["service.namespace"] = "fiap.postech",
                                ["service.instance.id"] = Environment.MachineName,
                                ["deployment.environment"] = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development"
                            }))
                        .AddSource("FiapPosTech.*")
                        .AddSource("Users.*")
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.Filter = (httpContext) =>
                            {
                                var path = httpContext.Request.Path.Value?.ToLower();
                                return !path?.Contains("/health") == true && 
                                       !path?.Contains("/metrics") == true;
                            };
                            
                            options.EnrichWithHttpRequest = (activity, httpRequest) =>
                            {
                                if (httpRequest.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
                                {
                                    activity.SetTag("correlation.id", correlationId.ToString());
                                }
                            };
                        })
                        .AddHttpClientInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddJaegerExporter(options =>
                        {
                            var jaegerEndpoint = configuration["OpenTelemetry:Jaeger:Endpoint"] ?? "http://jaeger:14268/api/traces";
                            options.Endpoint = new Uri(jaegerEndpoint);
                        });
                });

            services.AddSingleton(ActivitySource);
            return services;
        }

        public static Activity? StartActivity(string operationName, ActivityKind kind = ActivityKind.Internal)
        {
            return ActivitySource.StartActivity(operationName, kind);
        }

        public static void EnrichActivity(this Activity? activity, Dictionary<string, object> tags)
        {
            if (activity == null) return;
            foreach (var tag in tags)
            {
                activity.SetTag(tag.Key, tag.Value);
            }
        }

        public static void SetError(this Activity? activity, Exception exception)
        {
            if (activity == null) return;
            activity.SetStatus(ActivityStatusCode.Error, exception.Message);
            activity.SetTag("error", true);
            activity.SetTag("error.type", exception.GetType().Name);
            activity.SetTag("error.message", exception.Message);
        }

        public static void SetSuccess(this Activity? activity)
        {
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
    }
}