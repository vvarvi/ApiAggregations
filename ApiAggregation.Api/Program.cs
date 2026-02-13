using ApiAggregation.Application.Aggregation;
using ApiAggregation.Application.Aggregation.Interfaces;
using ApiAggregation.Infrastructure.Caching;
using ApiAggregation.Infrastructure.DependencyInjection;
using ApiAggregation.Infrastructure.ExternalApis;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.ExternalApis.GitHubApi;
using ApiAggregation.Infrastructure.ExternalApis.NewsApi;
using ApiAggregation.Infrastructure.ExternalApis.WeatherApi;
using ApiAggregation.Infrastructure.Logging;
using ApiAggregation.Infrastructure.Observability.Logging.Correlation;
using ApiAggregation.Infrastructure.Performance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry.Metrics;
using Prometheus;
using System.Text;

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
builder.Services.AddSingleton<ICacheTtlPolicy, ConfigurableCacheTtlPolicy>();

builder.Services.AddHttpClient<WeatherApiClient>();
builder.Services.AddHttpClient<NewsApiClient>();
builder.Services.AddHttpClient<GitHubApiClient>();

builder.Services.AddScoped<IExternalApiClient, WeatherApiClient>();
builder.Services.AddScoped<IExternalApiClient, NewsApiClient>();
builder.Services.AddScoped<IExternalApiClient, GitHubApiClient>();

builder.Services.Configure<ExternalApiOptions>(builder.Configuration.GetSection("ExternalApis"));

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSection["SecretKey"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // μόνο για development

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey))
        };
    });

//builder.Services.AddOpenTelemetry() // Replace AddOpenTelemetryMetrics with AddOpenTelemetry
//    .WithMetrics(otelBuilder =>
//    {
//        otelBuilder.AddMeter("ApiPerformanceMetrics");
//        otelBuilder.AddConsoleExporter();
//    });

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
