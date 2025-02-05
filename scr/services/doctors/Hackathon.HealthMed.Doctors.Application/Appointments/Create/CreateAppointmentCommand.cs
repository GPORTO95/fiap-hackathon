using Hackathon.HealthMed.Doctors.Domain.Appointments;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Domain.Patients;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Appointments.Create;

public sealed record CreateAppointmentCommand(
    Guid DoctorScheduleId,
    Guid PatientId) : ICommand;

internal sealed class CreateAppointmentCommandHandler(
    IDoctorScheduleRepository doctorScheduleRepository,
    IPatientRepository patientRepository,
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateAppointmentCommand>
{
    public async Task<Result> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        DoctorSchedule? schedule = await doctorScheduleRepository.GetByIdAsync(request.DoctorScheduleId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(DoctorScheduleErrors.NotFound);
        }

        if (!schedule.Available)
        {
            return Result.Failure(DoctorScheduleErrors.ScheduleIsNotFree);
        }

        if (!await patientRepository.ExistByIdAsync(request.PatientId, cancellationToken))
        {
            return Result.Failure(PatientErrors.NotFound);
        }

        Appointment appointment = Appointment.Create(Guid.NewGuid(), request.DoctorScheduleId, request.PatientId);

        appointmentRepository.Add(appointment);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
