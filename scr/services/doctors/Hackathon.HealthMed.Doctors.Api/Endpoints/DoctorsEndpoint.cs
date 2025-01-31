using Hackathon.HealthMed.Api.Core.Extensions;
using Hackathon.HealthMed.Doctors.Application.Doctors.AddSchedule;
using Hackathon.HealthMed.Doctors.Application.Doctors.Create;
using Hackathon.HealthMed.Doctors.Application.Doctors.Login;
using Hackathon.HealthMed.Kernel.Shared;
using MediatR;

namespace Hackathon.HealthMed.Doctors.Api.Endpoints;

public static class DoctorsEndpoint
{
    public static void MapDoctorEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/doctors/login", async (
            LoginDoctorCommand command,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<string> result = await sender.Send(command, CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
        
        app.MapPost("api/doctors", async (
            CreateDoctorCommand command,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<Guid> result = await sender.Send(command, CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
        
        app.MapPost("api/doctors/schedule", async (
            AddScheduleDoctorCommand command,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<Guid> result = await sender.Send(command, CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}