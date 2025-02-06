namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public sealed class DoctorSchedule
{
    public DoctorSchedule() { }
    
    private DoctorSchedule(Guid id, TimeStampRange time, Guid doctorId)
    {
        Id = id;
        DoctorId = doctorId;
        Time = time;
        Status = ScheduleStatus.Free;
    }

    public Guid Id { get; set; }

    public ScheduleStatus Status { get; set; }

    public TimeStampRange Time { get; set; }
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public Guid? PatientId { get; set; }

    public static DoctorSchedule Create(Guid id, TimeStampRange time, Guid doctorId)
    {
        return new DoctorSchedule(id, time, doctorId);
    }

    public void AddPatient(Guid patientId)
    {
        PatientId = patientId;
    }

    public void UpdateSchedule(TimeStampRange time)
    {
        Time = time;
        Status = ScheduleStatus.Free;
    }

    public bool IsAvailableForUpdate()
    {
        return IsAvailable() || Status == ScheduleStatus.Rejected;
    }

    public bool IsAvailable()
    {
        return Status == ScheduleStatus.Free;
    }

    public bool IsPending()
    {
        return Status == ScheduleStatus.Pending;
    }

    public void UpdateStatus(ScheduleStatus status)
    {
        Status = status;
    }
}
