using Hackathon.HealthMed.Doctors.Domain.Appointments;
using Hackathon.HealthMed.Doctors.Infrastructure.Data;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Repositories;

internal sealed class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
{
    public void Add(Appointment appointment)
    {
        context.Appointments.Add(appointment);
    }
}
