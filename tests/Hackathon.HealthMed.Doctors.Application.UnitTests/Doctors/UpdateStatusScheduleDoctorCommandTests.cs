using FluentAssertions;
using Hackathon.HealthMed.Doctors.Application.Doctors.UpdateStatusSchedule;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Doctors;

public class UpdateStatusScheduleDoctorCommandTests
{
    private readonly UpdateStatusScheduleDoctorCommand Command = new(Guid.NewGuid(), true);

    private readonly IDoctorScheduleRepository _doctorScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly UpdateStatusScheduleDoctorCommandHandler _handler;

    public UpdateStatusScheduleDoctorCommandTests()
    {
        _doctorScheduleRepository = Substitute.For<IDoctorScheduleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateStatusScheduleDoctorCommandHandler(_doctorScheduleRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenScheduleNotFound_ShouldReturnNotFoundFailure()
    {
        // Arrange
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns((DoctorSchedule)null);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DoctorScheduleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_WhenScheduleIsNotPending_ShouldReturnIsNotPendingFailure()
    {
        // Arrange
        var schedule = new DoctorSchedule { Status = ScheduleStatus.Accepted }; // Assuming ScheduleStatus.Accepted is not pending
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DoctorScheduleErrors.IsNotPending);
    }

    [Fact]
    public async Task Handle_WhenScheduleIsPendingAndStatusIsAccepted_ShouldUpdateStatusToAcceptedAndReturnSuccess()
    {
        // Arrange
        var schedule = new DoctorSchedule { Status = ScheduleStatus.Pending };
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        schedule.Status.Should().Be(ScheduleStatus.Accepted);
        _doctorScheduleRepository.Received(1).Update(schedule);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenScheduleIsPendingAndStatusIsRejected_ShouldUpdateStatusToRejectedAndReturnSuccess()
    {
        // Arrange
        var request = Command with { Status = false };
        var schedule = new DoctorSchedule { Status = ScheduleStatus.Pending }; // Assuming ScheduleStatus.Pending is pending
        _doctorScheduleRepository.GetByIdAsync(request.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        schedule.Status.Should().Be(ScheduleStatus.Rejected);
        _doctorScheduleRepository.Received(1).Update(schedule);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
