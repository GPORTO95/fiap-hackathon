using Hackathon.HealthMed.Api.Core.Handlers;
using Hackathon.HealthMed.Doctors.Api.Endpoints;
using Hackathon.HealthMed.Doctors.Application;
using Hackathon.HealthMed.Doctors.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string contentRoot = builder.Environment.ContentRootPath;

string environmentName = builder.Environment.EnvironmentName;

Console.WriteLine($"environment utilizado : {environmentName}");


ILoggerFactory loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration,loggerFactory);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Configuration.AddEnvironmentVariables();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithTitle("Hackathon Health and Med");
        //.WithApiKeyAuthentication(keyOptions => keyOptions.Token);
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapDoctorEndpoints();

app.Run();

public partial class Program { }