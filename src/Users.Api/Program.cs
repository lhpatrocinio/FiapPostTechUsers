using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Users.Api.Extensions.Auth.Middleware;
using Users.Api.Extensions.Auth;
using Users.Api.Extensions.Logs.Extension;
using Users.Api.Extensions.Logs.ELK;
using Users.Api.Extensions.Logs;
using Users.Api.Extensions.Migration;
using Users.Api.Extensions.Swagger.Extension;
using Users.Api.Extensions.Swagger.Middleware;
using Users.Api.Extensions.Tracing;
using Users.Api.Extensions.Versioning.Extension;
using Users.Application.Mappers;
using Users.Application;
using Users.Infrastructure.DataBase.EntityFramework.Context;
using Users.Infrastructure;
using Users.Infrastructure.DataBase.EntityFramework.Identity.Extension;
using Users.Infrastructure.Monitoring;
using Users.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogConfiguration();
builder.WebHost.UseUrls("http://*:80");

builder.Services.AddMvcCore(options => options.AddLogRequestFilter());
builder.Services.AddVersioning();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentityExtension();
builder.Services.AddAuthorizationExtension(builder.Configuration);

// Adiciona configuração CORS para permitir solicitações do Prometheus
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Adiciona monitoramento com Prometheus
builder.Services.AddPrometheusMonitoring();
builder.Services.AddSingleton<MetricsCollector>();

// ELK Stack integration
builder.Services.AddELKIntegration(builder.Configuration);

// Distributed Tracing with OpenTelemetry + Jaeger
builder.Services.AddDistributedTracing(builder.Configuration);

#region [DI]

ApplicationBootstrapper.Register(builder.Services);
InfraBootstrapper.Register(builder.Services);

#endregion

#region [Consumers]

builder.Services.AddHostedService<UserActiveConsumer>();

#endregion
var app = builder.Build();

// Log manual para teste
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Users API iniciada - Teste de log manual com metadados");

app.ExecuteMigrations();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseAuthentication();                        // 1°: popula HttpContext.User
app.UseMiddleware<RoleAuthorizationMiddleware>(); // 2°: seu middleware
app.UseCorrelationId();
app.UseELKIntegration();

// Adiciona CORS antes de outros middlewares
app.UseCors("AllowAll");

// Adiciona middleware de monitoramento
app.UsePrometheusMonitoring();

// Adiciona request logging com Serilog
app.UseSerilogRequestLogging();

app.UseVersionedSwagger(apiVersionDescriptionProvider);
app.UseAuthorization();                         // 3°: aplica [Authorize]
app.MapControllers();
app.Run();