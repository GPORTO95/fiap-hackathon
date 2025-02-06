using FluentAssertions;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patients.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Patients.Application.Patients.Login;
using Hackathon.HealthMed.Patients.Domain.Patients;
using NSubstitute;

namespace Hackathon.HealthMed.Patients.Application.UnitTests;

public class LoginPatientCommandTests
{
    private readonly LoginPatientCommand CommandByEmail = new("test@test.com", null, "Teste@123");
    private readonly LoginPatientCommand CommandByCpf = new(null, "83351678002", "Teste@123");

    private readonly IPatientRepository _patientRepository;
    private readonly IJwtProvider _jwtProvider;
    
    private readonly LoginPatientCommandHandler _handler;

    public LoginPatientCommandTests()
    {
        _patientRepository = Substitute.For<IPatientRepository>();
        _jwtProvider = Substitute.For<IJwtProvider>();
        
        _handler = new (_patientRepository, _jwtProvider);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotValid()
    {
        // Arrange
        LoginPatientCommand invalidCommand = CommandByEmail with
        {
            Email = "test"
        };

        // Act
        Result<string> result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenCpfIsNotValid()
    {
        // Arrange
        LoginPatientCommand invalidCommand = CommandByEmail with
        {
            Cpf = "11122233344",
            Email = null
        };

        // Act
        Result<string> result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CpfErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenPassowrdIsNotValid()
    {
        // Arrange
        LoginPatientCommand invalidCommand = CommandByEmail with
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
    public async Task Handle_Should_ReturnError_WhenLoginByEmailIsInvalid()
    {
        // Arrange
        MockLoginEmail(false);
        
        // Act
        Result<string> result = await _handler.Handle(CommandByEmail, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.LoginInvalid);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenLoginByCpfIsInvalid()
    {
        // Arrange
        MockLoginCpf(false);

        // Act
        Result<string> result = await _handler.Handle(CommandByEmail, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.LoginInvalid);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenLoginByEmailIsValid()
    {
        // Arrange
        MockLoginEmail();
        
        // Act
        Result<string> result = await _handler.Handle(CommandByEmail, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<string>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenLoginByCpfIsValid()
    {
        // Arrange
        MockLoginCpf();

        // Act
        Result<string> result = await _handler.Handle(CommandByCpf, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<string>();
    }

    private void MockLoginEmail(bool exist = true)
    {
        Domain.Patients.Patient? patient = exist 
            ? new Patient(
                Guid.NewGuid(), 
                Name.Create("Gabriel").Value, 
                Email.Create("test@test.com").Value, 
                Cpf.Create("47101894046").Value,
                Password.Create("Teste@123").Value)
            : null;
        
        _patientRepository.LoginByEmailAsync(
            Arg.Any<Email>(),
            Arg.Any<Password>(),
            default)
            .Returns(patient);
    }

    private void MockLoginCpf(bool exist = true)
    {
        Domain.Patients.Patient? patient = exist
            ? new Patient(
                Guid.NewGuid(),
                Name.Create("Gabriel").Value,
                Email.Create("test@test.com").Value,
                Cpf.Create("47101894046").Value,
                Password.Create("Teste@123").Value)
            : null;

        _patientRepository.LoginByCpfAsync(
            Arg.Any<Cpf>(),
            Arg.Any<Password>(),
            default)
            .Returns(patient);
    }
}