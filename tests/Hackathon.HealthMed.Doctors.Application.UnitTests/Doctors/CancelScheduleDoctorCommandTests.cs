using Hackathon.HealthMed.Doctors.Application.Doctors.CancelSchedule;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Doctors;

public class CancelScheduleDoctorCommandTests
{
    private readonly CancelScheduleDoctorCommand Command = new(Guid.NewGuid(), "cancel");

    private readonly CancelScheduleDoctorCommandHandler _handler;
    private readonly IDoctorScheduleRepository _doctorScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelScheduleDoctorCommandTests()
    {
        _doctorScheduleRepository = Substitute.For<IDoctorScheduleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CancelScheduleDoctorCommandHandler(_doctorScheduleRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenScheduleNotFound_ReturnsFailure()
    {
        // Arrange
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns((DoctorSchedule?)null);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DoctorScheduleErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WhenScheduleIsNotPending_ReturnsFailure()
    {
        // Arrange
        var schedule = new DoctorSchedule { Status = ScheduleStatus.Accepted }; // Not pending
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DoctorScheduleErrors.IsNotPending, result.Error);
    }

    [Fact]
    public async Task Handle_WhenScheduleIsPending_UpdatesStatusAndSavesChanges()
    {
        // Arrange
        var schedule = new DoctorSchedule { Status = ScheduleStatus.Pending }; // Pending schedule
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ScheduleStatus.Canceled, schedule.Status);
        Assert.Equal(Command.Reason, schedule.Reason);
        _doctorScheduleRepository.Received(1).Update(schedule);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
