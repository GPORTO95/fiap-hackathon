using FluentAssertions;
using Hackathon.HealthMed.Doctors.Application.Appointments.Create;
using Hackathon.HealthMed.Doctors.Domain.Appointments;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Domain.Patients;
using Hackathon.HealthMed.Kernel.Data;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Appointments;

public class CreateAppointmentCommandTests
{
    private readonly IDoctorScheduleRepository _doctorScheduleRepositoryMock = Substitute.For<IDoctorScheduleRepository>();
    private readonly IPatientRepository _patientRepositoryMock = Substitute.For<IPatientRepository>();
    private readonly IAppointmentRepository _appointmentRepositoryMock = Substitute.For<IAppointmentRepository>();
    private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();

    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandTests()
    {
        _handler = new CreateAppointmentCommandHandler(
            _doctorScheduleRepositoryMock,
            _patientRepositoryMock,
            _appointmentRepositoryMock,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Handle_WhenDoctorScheduleNotFound_ShouldReturnFailure()
    {
        // Arrange
        var request = new CreateAppointmentCommand(Guid.NewGuid(), Guid.NewGuid());
        _doctorScheduleRepositoryMock.GetByIdAsync(request.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns((DoctorSchedule)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DoctorScheduleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_WhenDoctorScheduleIsNotAvailable_ShouldReturnFailure()
    {
        // Arrange
        var request = new CreateAppointmentCommand(Guid.NewGuid(), Guid.NewGuid());
        var schedule = new DoctorSchedule { Available = false };
        _doctorScheduleRepositoryMock.GetByIdAsync(request.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DoctorScheduleErrors.ScheduleIsNotFree);
    }

    [Fact]
    public async Task Handle_WhenPatientNotFound_ShouldReturnFailure()
    {
        // Arrange
        var request = new CreateAppointmentCommand(Guid.NewGuid(), Guid.NewGuid());
        var schedule = new DoctorSchedule { Available = true };
        _doctorScheduleRepositoryMock.GetByIdAsync(request.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);
        _patientRepositoryMock.ExistByIdAsync(request.PatientId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PatientErrors.NotFound);
    }

    [Fact]
    public async Task Handle_WhenAllConditionsAreMet_ShouldReturnSuccess()
    {
        // Arrange
        var request = new CreateAppointmentCommand(Guid.NewGuid(), Guid.NewGuid());
        var schedule = new DoctorSchedule { Available = true };
        _doctorScheduleRepositoryMock.GetByIdAsync(request.DoctorScheduleId, Arg.Any<CancellationToken>()).Returns(schedule);
        _patientRepositoryMock.ExistByIdAsync(request.PatientId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _appointmentRepositoryMock.Received(1).Add(Arg.Any<Appointment>());
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
