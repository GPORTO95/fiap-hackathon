using Hackathon.HealthMed.Doctors.Application.Doctors.UpdateSchedule;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Doctors;

public class UpdateScheduleDoctorCommandTests
{
    private readonly UpdateScheduleDoctorCommand Command = new(
        Guid.NewGuid(),
        DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
        new TimeSpan(10, 0, 0),
        new TimeSpan(12, 0, 0));

    private readonly IDoctorScheduleRepository _doctorScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateScheduleDoctorCommandHandler _handler;

    public UpdateScheduleDoctorCommandTests()
    {
        _doctorScheduleRepository = Substitute.For<IDoctorScheduleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new UpdateScheduleDoctorCommandHandler(_doctorScheduleRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenTimeStampRangeIsInvalid_ReturnsFailure()
    {
        // Arrange
        var invalidCommand = Command with
        {
            Start = TimeSpan.FromHours(10), // Invalid range (start > end)
            End = TimeSpan.FromHours(9)
        };

        // Act
        var result = await _handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task Handle_WhenScheduleNotFound_ReturnsFailure()
    {
        // Arrange
        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns((DoctorSchedule)null);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DoctorScheduleErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WhenScheduleIsNotFree_ReturnsFailure()
    {
        // Arrange
        var schedule = new DoctorSchedule { DoctorId = Guid.NewGuid() };

        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);
        _doctorScheduleRepository.ScheduleIsFreeAsync(Command.Date, Command.Start, Command.End, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DoctorScheduleErrors.ScheduleIsNotFree, result.Error);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsSuccess()
    {
        // Arrange
        var schedule = new DoctorSchedule { DoctorId = Guid.NewGuid() };

        _doctorScheduleRepository.GetByIdAsync(Command.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);
        _doctorScheduleRepository.ScheduleIsFreeAsync(Command.Date, Command.Start, Command.End, Arg.Any<CancellationToken>()).Returns(true);


        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _doctorScheduleRepository.Received(1).Update(schedule);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
