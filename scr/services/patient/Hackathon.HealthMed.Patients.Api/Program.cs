using Hackathon.HealthMed.Api.Core.Handlers;
using Hackathon.HealthMed.Patients.Api.Endpoints;
using Hackathon.HealthMed.Patients.Application;
using Hackathon.HealthMed.Patients.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string contentRoot = builder.Environment.ContentRootPath;

string environmentName = builder.Environment.EnvironmentName;

Console.WriteLine($"environment utilizado : {environmentName}");


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

ILoggerFactory loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});


builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, loggerFactory);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

// app.UseScalar(options =>
// {
//     options.RoutePrefix = "docs";
// });

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapPatientEndpoints();

app.Run();

public partial class Program { }