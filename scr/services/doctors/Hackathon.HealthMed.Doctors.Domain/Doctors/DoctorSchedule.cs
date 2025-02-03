
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public sealed class DoctorSchedule
{
    public DoctorSchedule() { }
    
    private DoctorSchedule(Guid id, TimeStampRange time, Guid doctorId)
    {
        Id = id;
        DoctorId = doctorId;
        Time = time;
        Available = true;
    }

    public Guid Id { get; set; }

    public bool Available { get; set; }

    public TimeStampRange Time { get; set; }
    
    public Guid DoctorId { get; set; }
    
    public static DoctorSchedule Create(Guid id, TimeStampRange time, Guid doctorId)
    {
        return new DoctorSchedule(id, time, doctorId);
    }

    public void UpdateSchedule(TimeStampRange time)
    {
        Time = time;
    }
}

public static class DoctorScheduleErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "DoctorSchedule.NotFound",
        "Doctor schedule not found.");

    public static readonly Error ScheduleIsNotFree = Error.Conflict(
        "DoctorSchedule.ScheduleIsNotFree",
        "Doctor schedule is not free.");
}