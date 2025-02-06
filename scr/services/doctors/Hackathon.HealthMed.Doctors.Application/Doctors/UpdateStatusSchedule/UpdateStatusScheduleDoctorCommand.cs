using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.UpdateStatusSchedule;

public sealed record UpdateStatusScheduleDoctorCommand(
    Guid DoctorScheduleId,
    bool Status) : ICommand;

internal sealed class UpdateStatusScheduleDoctorCommandHandler(
    IDoctorScheduleRepository doctorScheduleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateStatusScheduleDoctorCommand>
{
    public async Task<Result> Handle(UpdateStatusScheduleDoctorCommand request, CancellationToken cancellationToken)
    {
        DoctorSchedule? schedule = await doctorScheduleRepository.GetByIdAsync(request.DoctorScheduleId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(DoctorScheduleErrors.NotFound);
        }

        if (!schedule.IsPending())
        {
            return Result.Failure(DoctorScheduleErrors.IsNotPending);
        }

        ScheduleStatus scheduleStatus = request.Status
            ? ScheduleStatus.Accepted
            : ScheduleStatus.Rejected;

        schedule.UpdateStatus(scheduleStatus);

        doctorScheduleRepository.Update(schedule);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
