using FluentAssertions;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patient.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Patient.Application.Patients.Login;
using Hackathon.HealthMed.Patient.Domain.Patients;
using NSubstitute;

namespace Hackathon.HealthMed.Patient.Application.UnitTests;

public class LoginPatientCommandTests
{
    private readonly LoginPatientCommand Command = new("test@test.com", "Teste@123");
    
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
        LoginPatientCommand invalidCommand = Command with
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
    public async Task Handle_Should_ReturnError_WhenPassowrdIsNotValid()
    {
        // Arrange
        LoginPatientCommand invalidCommand = Command with
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
        result.Error.Should().Be(PatientErrors.LoginInvalid);
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
        Domain.Patients.Patient? patient = exist 
            ? new Domain.Patients.Patient(
                Guid.NewGuid(), 
                Name.Create("Gabriel").Value, 
                Email.Create("test@test.com").Value, 
                Cpf.Create("47101894046").Value,
                Password.Create("Teste@123").Value)
            : null;
        
        _patientRepository.LoginAsync(
            Arg.Any<Email>(),
            Arg.Any<Password>(),
            default)
            .Returns(patient);
    }
}