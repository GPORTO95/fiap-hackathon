namespace Hackathon.HealthMed.Doctors.Domain.Appointments;

public sealed class Appointment
{
    protected Appointment()
    { }

    private Appointment(Guid id, Guid doctorScheduleId, Guid patientId)
    {
        Id = id;
        DoctorScheduleId = doctorScheduleId;
        PatientId = patientId;
    }

    public Guid Id { get; set; }

    public Guid DoctorScheduleId { get; set; }

    public Guid PatientId { get; set; }

    public static Appointment Create(Guid id, Guid doctorScheduleId, Guid patientId)
    {
        return new Appointment(id, doctorScheduleId, patientId);
    }
}
