using AspNetCore.Scalar;
using Hackathon.HealthMed.Api.Core.Handlers;
using Hackathon.HealthMed.Patient.Api.Endpoints;
using Hackathon.HealthMed.Patient.Application;
using Hackathon.HealthMed.Patient.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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