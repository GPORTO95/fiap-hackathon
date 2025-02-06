using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.CancelSchedule;

public sealed record CancelScheduleDoctorRequest(string Reason);

public sealed record CancelScheduleDoctorCommand(
    Guid DoctorScheduleId,
    string Reason) : ICommand;

internal sealed class CancelScheduleDoctorCommandHandler(
    IDoctorScheduleRepository doctorScheduleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CancelScheduleDoctorCommand>
{
    public async Task<Result> Handle(CancelScheduleDoctorCommand request, CancellationToken cancellationToken)
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

        schedule.UpdateStatus(ScheduleStatus.Canceled, request.Reason);

        doctorScheduleRepository.Update(schedule);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
