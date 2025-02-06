using Hackathon.HealthMed.Api.Core.Handlers;
using Hackathon.HealthMed.Doctors.Api.Endpoints;
using Hackathon.HealthMed.Doctors.Application;
using Hackathon.HealthMed.Doctors.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

//builder.Services.AddMvc();

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

app.UseHttpsRedirection();

app.UseExceptionHandler();

//app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDoctorEndpoints();

app.Run();

public partial class Program { }