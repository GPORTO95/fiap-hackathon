namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public interface IDoctorScheduleRepository
{
    Task<bool> ScheduleIsFreeAsync(DateOnly date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken = default);

    void Add(DoctorSchedule doctorSchedule);
}