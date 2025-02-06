namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public interface IDoctorScheduleRepository
{
    Task<bool> ScheduleIsFreeAsync(Guid doctorId, DateOnly date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken = default);

    Task<List<DoctorSchedule>> GetAvailableByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken = default);

    Task<DoctorSchedule?> GetByIdAsync(Guid doctorScheduleId, CancellationToken cancellationToken = default);

    void Add(DoctorSchedule doctorSchedule);

    void Update(DoctorSchedule doctorSchedule);
}