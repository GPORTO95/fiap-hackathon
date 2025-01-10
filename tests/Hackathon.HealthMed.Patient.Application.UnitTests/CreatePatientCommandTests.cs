using FluentAssertions;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patient.Application.Patients.Create;
using Hackathon.HealthMed.Patient.Domain.Patients;
using NSubstitute;

namespace Hackathon.HealthMed.Patient.Application.UnitTests;

public class CreatePatientCommandTests
{
    private readonly CreatePatientCommand Command = new("Test", "test@gmail.com", "73723110045", "Wtf1203*");

    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly CreatePatientCommandHandler _handler;

    public CreatePatientCommandTests()
    {
        _patientRepository = Substitute.For<IPatientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        
        _handler = new CreatePatientCommandHandler(_patientRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenNameIsNotValid()
    {
        // Arrange
        CreatePatientCommand invalidCommand = Command with
        {
            Name = string.Empty
        };
        
        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(NameErrors.Empty);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotValid()
    {
        // Arrange
        CreatePatientCommand invalidCommand = Command with
        {
            Email = "test"
        };
        
        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenCpfIsNotValid()
    {
        // Arrange
        CreatePatientCommand invalidCommand = Command with
        {
            Cpf = "11122233344"
        };
        
        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CpfErrors.InvalidFormat);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenCpfNotUnique()
    {
        // Arrange
        MockCpf(false);
        
        // Act
        Result<Guid> result = await _handler.Handle(Command, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.CpfNotUnique);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnPassword_WhenCpfIsNotValid()
    {
        // Arrange
        MockCpf();
        
        CreatePatientCommand invalidCommand = Command with
        {
            Password = "Teste123"
        };
        
        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PasswordErrors.EmptySpecialChar);
    }

    private void MockCpf(bool isUnique = true)
    {
        _patientRepository.IsCpfUniqueAsync(
            Cpf.Create(Command.Cpf).Value, default)
            .Returns(isUnique);
    }
}