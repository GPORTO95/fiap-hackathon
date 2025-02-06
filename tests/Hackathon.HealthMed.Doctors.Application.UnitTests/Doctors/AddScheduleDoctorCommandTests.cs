using Hackathon.HealthMed.Doctors.Application.Doctors.AddSchedule;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Doctors;

public class AddScheduleDoctorCommandTests
{
    private readonly AddScheduleDoctorCommand Command = new(
        Guid.NewGuid(), 
        DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
        new TimeSpan(10, 0, 0),
        new TimeSpan(12, 0, 0));

    private readonly IDoctorRepository _doctorRepository;
    private readonly IDoctorScheduleRepository _doctorScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly AddScheduleDoctorCommandHandler _handler;

    public AddScheduleDoctorCommandTests()
    {
        _doctorRepository = Substitute.For<IDoctorRepository>();
        _doctorScheduleRepository = Substitute.For<IDoctorScheduleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new AddScheduleDoctorCommandHandler(
            _doctorRepository,
            _doctorScheduleRepository,
            _unitOfWork
        );
    }

    [Fact]
    public async Task Handle_WhenTimeStampRangeIsInvalid_ShouldReturnFailure()
    {
        // Arrange
        var invalidCommand = Command with { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), };

        // Act
        var result = await _handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeStampRangeErrors.DateInvalid, result.Error);
    }

    [Fact]
    public async Task Handle_WhenDoctorDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        _doctorRepository.ExistByIdAsync(Command.DoctorId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DoctorErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WhenScheduleIsNotFree_ShouldReturnFailure()
    {
        // Arrange
        _doctorRepository.ExistByIdAsync(Command.DoctorId, Arg.Any<CancellationToken>()).Returns(true);
        _doctorScheduleRepository.ScheduleIsFreeAsync(Command.DoctorId, Command.Date, Command.Start, Command.End, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DoctorScheduleErrors.IsNotFree, result.Error);
    }

    [Fact]
    public async Task Handle_WhenAllConditionsAreMet_ShouldReturnSuccess()
    {
        // Arrange
        _doctorRepository.ExistByIdAsync(Command.DoctorId, Arg.Any<CancellationToken>()).Returns(true);
        _doctorScheduleRepository.ScheduleIsFreeAsync(Command.DoctorId, Command.Date, Command.Start, Command.End, Arg.Any<CancellationToken>()).Returns(true);
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(Command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
    }
}
