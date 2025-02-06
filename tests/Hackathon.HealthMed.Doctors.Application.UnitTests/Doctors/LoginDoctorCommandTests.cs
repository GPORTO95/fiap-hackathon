using FluentAssertions;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Doctors.Application.Doctors.Login;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Doctors;

public class LoginDoctorCommandTests
{
    private readonly LoginDoctorCommand Command = new("123456", "Teste@123");
    
    private readonly IDoctorRepository _doctorRepositoryMock;
    private readonly IJwtProvider _jwtProviderMock;
    
    private readonly LoginDoctorCommandHandler _handler;

    public LoginDoctorCommandTests()
    {
        _doctorRepositoryMock = Substitute.For<IDoctorRepository>();
        _jwtProviderMock = Substitute.For<IJwtProvider>();
        
        _handler = new (_doctorRepositoryMock, _jwtProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotValid()
    {
        // Arrange
        var invalidCommand = Command with
        {
            Crm = "test"
        };

        // Act
        Result<string> result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CrmErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenPassowrdIsNotValid()
    {
        // Arrange
        var invalidCommand = Command with
        {
            Password = "Teste123"
        };

        // Act
        Result<string> result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PasswordErrors.EmptySpecialChar);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenLoginIsInvalid()
    {
        // Arrange
        MockLogin(false);
        
        // Act
        Result<string> result = await _handler.Handle(Command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DoctorErrors.LoginInvalid);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenLoginValid()
    {
        // Arrange
        MockLogin();
        
        // Act
        Result<string> result = await _handler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<string>();
    }

    private void MockLogin(bool exist = true)
    {
        Doctor? doctor = exist
            ? new Doctor(
                Guid.NewGuid(),
                Name.Create("Gabriel").Value,
                Email.Create("test@test.com").Value,
                Cpf.Create("47101894046").Value,
                Crm.Create("123456").Value,
                Password.Create("Teste@123").Value,
                Specialty.GeneralPractice)
            : null;
        
        _doctorRepositoryMock.LoginAsync(
            Arg.Any<Crm>(),
            Arg.Any<Password>(),
            default)
            .Returns(doctor);
    }
}