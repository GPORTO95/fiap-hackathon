using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Domain.Patients;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.AddAppointment;


public sealed record AddAppointmentCommand(
    Guid DoctorScheduleId,
    Guid PatientId) : ICommand;

internal sealed class AddAppointmentCommandHandler(
    IDoctorScheduleRepository doctorScheduleRepository,
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddAppointmentCommand>
{
    public async Task<Result> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
    {
        DoctorSchedule? schedule = await doctorScheduleRepository.GetByIdAsync(request.DoctorScheduleId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(DoctorScheduleErrors.NotFound);
        }

        if (!schedule.IsAvailable())
        {
            return Result.Failure(DoctorScheduleErrors.IsNotFree);
        }

        if (!await patientRepository.ExistByIdAsync(request.PatientId, cancellationToken))
        {
            return Result.Failure(PatientErrors.NotFound);
        }

        schedule.UpdateStatus(ScheduleStatus.Pending);
        schedule.AddPatient(request.PatientId);

        doctorScheduleRepository.Update(schedule);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}