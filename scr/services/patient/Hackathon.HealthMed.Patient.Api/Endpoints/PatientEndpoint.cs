using Hackathon.HealthMed.Api.Core.Extensions;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patient.Application.Patients.Create;
using Hackathon.HealthMed.Patient.Application.Patients.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.HealthMed.Patient.Api.Endpoints;

public static class PatientEndpoint
{
    public static void MapPatientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/patients/login", async (
            LoginPatientCommand command,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<string> result = await sender.Send(command, CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
        
        app.MapPost("api/patients", async (
            CreatePatientCommand command,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<Guid> result = await sender.Send(command, CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}