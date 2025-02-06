using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.AvailableSchedule;

public sealed record AvailableScheduleDoctorQuery(Guid DoctorId) : IQuery<IEnumerable<AvailableScheduleDoctorQueryResponse>>;

public sealed record AvailableScheduleDoctorQueryResponse(
    Guid DoctorScheduleId,
    DateOnly Date,
    TimeSpan Start,
    TimeSpan End,
    decimal Price);

internal sealed class AvailableScheduleDoctorQueryHandler(IDoctorRepository doctorRepository, IDoctorScheduleRepository doctorScheduleRepository) : IQueryHandler<AvailableScheduleDoctorQuery, IEnumerable<AvailableScheduleDoctorQueryResponse>>
{
    public async Task<Result<IEnumerable<AvailableScheduleDoctorQueryResponse>>> Handle(AvailableScheduleDoctorQuery request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await doctorRepository.GetByIdAsync(request.DoctorId, cancellationToken);

        if (doctor is null)
        {
            return Result.Failure<IEnumerable<AvailableScheduleDoctorQueryResponse>>(DoctorErrors.NotFound);
        }

        List<DoctorSchedule> schedules = await doctorScheduleRepository.GetAvailableByDoctorIdAsync(request.DoctorId, cancellationToken);

        if (schedules.Count == 0)
        {
            return Array.Empty<AvailableScheduleDoctorQueryResponse>();
        }

        return schedules
            .Select(s => new AvailableScheduleDoctorQueryResponse(
                s.Id,
                s.Time.Date,
                s.Time.Start,
                s.Time.End,
                s.Price))
            .ToList();
    }
}