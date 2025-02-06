using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.UpdateSchedule;

public sealed record UpdateScheduleDoctorRequest(
    DateOnly Date,
    TimeSpan Start,
    TimeSpan End,
    decimal Price);

public sealed record UpdateScheduleDoctorCommand(
    Guid DoctorScheduleId,
    DateOnly Date,
    TimeSpan Start,
    TimeSpan End,
    decimal Price) : ICommand;

internal sealed class UpdateScheduleDoctorCommandHandler(
    IDoctorScheduleRepository doctorScheduleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateScheduleDoctorCommand>
{
    public async Task<Result> Handle(UpdateScheduleDoctorCommand request, CancellationToken cancellationToken)
    {
        Result<TimeStampRange> rangeResult = TimeStampRange.Create(request.Date, request.Start, request.End);

        if (rangeResult.IsFailure)
        {
            return Result.Failure<Guid>(rangeResult.Error);
        }

        DoctorSchedule? schedule = await doctorScheduleRepository.GetByIdAsync(request.DoctorScheduleId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(DoctorScheduleErrors.NotFound);
        }

        if (!schedule.IsAvailableForUpdate())
        {
            return Result.Failure(DoctorScheduleErrors.Denied);
        }

        if (!await doctorScheduleRepository.ScheduleIsFreeAsync(schedule.DoctorId, request.Date, request.Start, request.End, cancellationToken))
        {
            return Result.Failure<Guid>(DoctorScheduleErrors.IsNotFree);
        }

        schedule.UpdateSchedule(rangeResult.Value, request.Price);

        doctorScheduleRepository.Update(schedule);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}