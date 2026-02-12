using ApiAggregation.Application.Aggregation;
using ApiAggregation.Application.Aggregation.Interfaces;
using ApiAggregation.Infrastructure.DependencyInjection;
using ApiAggregation.Infrastructure.ExternalApis.GitHubApi;
using ApiAggregation.Infrastructure.ExternalApis.NewsApi;
using ApiAggregation.Infrastructure.ExternalApis.WeatherApi;
using ApiAggregation.Infrastructure.Logging;
using ApiAggregation.Infrastructure.Observability.Logging.Correlation;
using Microsoft.OpenApi;
using Prometheus;
using ApiAggregation.Infrastructure.Performance;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<NewsApiClient>(c =>
{
    c.BaseAddress = new Uri("https://newsapi.org");
});

builder.Services.AddHttpClient<WeatherApiClient>(c =>
{
    c.BaseAddress = new Uri("https://api.openweathermap.org");
});

builder.Services.AddHttpClient<GitHubApiClient>(c =>
{
    c.BaseAddress = new Uri("https://api.github.com");
});

builder.Services.AddScoped<IAggregationService, AggregationService>();

//builder.Services.AddScoped<IExternalApiClient>(sp =>
//{
//    var httpClient = sp.GetRequiredService<HttpClient>();
//    var memoryCache = sp.GetRequiredService<IMemoryCache>();

//    var realClient = ActivatorUtilities.CreateInstance<NewsApiClient>(sp);

//    return new CachedExternalApiClient(realClient, memoryCache);
//});


var configuration = builder.Configuration;

//builder.Services.AddHttpClient();

builder.Services.AddInfrastructure(builder.Configuration);

LoggingConfiguration.ConfigureSerilog(builder);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Aggregation",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme"
    });

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new List<string>()
    //    }
    //});
});

builder.Services.AddSingleton<IApiPerformanceTracker, InMemoryApiPerformanceTracker>();
builder.Services.AddHostedService<PerformanceAnalyzerHostedService>();

builder.Services.AddOpenTelemetry() // Replace AddOpenTelemetryMetrics with AddOpenTelemetry
    .WithMetrics(otelBuilder =>
    {
        otelBuilder.AddMeter("ApiPerformanceMetrics");
        otelBuilder.AddConsoleExporter();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();

app.UseHttpMetrics();

app.MapMetrics("/metrics");

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
