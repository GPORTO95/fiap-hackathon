using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.AddSchedule;

public sealed record AddScheduleDoctorCommand(
    Guid DoctorId,
    DateOnly Date,
    TimeSpan Start,
    TimeSpan End) : ICommand<Guid>;

internal sealed class AddScheduleDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IDoctorScheduleRepository doctorScheduleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddScheduleDoctorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddScheduleDoctorCommand request, CancellationToken cancellationToken)
    {
        Result<TimeStampRange> rangeResult = TimeStampRange.Create(request.Date, request.Start, request.End);

        if (rangeResult.IsFailure)
        {
            return Result.Failure<Guid>(rangeResult.Error);
        }

        if (!await doctorRepository.ExistByIdAsync(request.DoctorId, cancellationToken))
        {
            return Result.Failure<Guid>(DoctorErrors.NotFound);
        }

        if (!await doctorScheduleRepository.ScheduleIsFreeAsync(request.DoctorId, request.Date, request.Start, request.End, cancellationToken))
        {
            return Result.Failure<Guid>(DoctorScheduleErrors.IsNotFree);
        }

        DoctorSchedule schedule = DoctorSchedule.Create(
            Guid.NewGuid(),
            rangeResult.Value,
            request.DoctorId);

        doctorScheduleRepository.Add(schedule);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return schedule.Id;
    }
}