using Hackathon.HealthMed.Api.Core.Extensions;
using Hackathon.HealthMed.Doctors.Application.Appointments.Create;
using Hackathon.HealthMed.Doctors.Application.Doctors.AddSchedule;
using Hackathon.HealthMed.Doctors.Application.Doctors.AvailableSchedule;
using Hackathon.HealthMed.Doctors.Application.Doctors.Create;
using Hackathon.HealthMed.Doctors.Application.Doctors.ListPaged;
using Hackathon.HealthMed.Doctors.Application.Doctors.Login;
using Hackathon.HealthMed.Doctors.Application.Doctors.UpdateSchedule;
using Hackathon.HealthMed.Kernel.Shared;
using MediatR;

namespace Hackathon.HealthMed.Doctors.Api.Endpoints;

public static class DoctorsEndpoint
{
    public static void MapDoctorEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/doctors/search", async (
            ListPagedDoctorQuery query,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<PagedList<ListPagedDoctorQueryResponse>> result = await sender.Send(query, CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        app.MapGet("api/doctors/{doctorId}/available-schedule", async (
            Guid doctorId,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result<IEnumerable<AvailableScheduleDoctorQueryResponse>> result = await sender.Send(new AvailableScheduleDoctorQuery(doctorId), CancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }); 

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

        app.MapPut("api/doctors/{doctorScheduleId}/schedule", async (
            Guid doctorScheduleId,
            UpdateScheduleDoctorRequest request,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result result = await sender.Send(new UpdateScheduleDoctorCommand(doctorScheduleId, request.Date, request.Start, request.End), CancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });

        app.MapPost("api/doctors/{doctorScheduleId}/{patientId}/appointment", async (
            Guid doctorScheduleId,
            Guid patientId,
            ISender sender,
            CancellationToken CancellationToken) =>
        {
            Result result = await sender.Send(new CreateAppointmentCommand(doctorScheduleId, patientId), CancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}