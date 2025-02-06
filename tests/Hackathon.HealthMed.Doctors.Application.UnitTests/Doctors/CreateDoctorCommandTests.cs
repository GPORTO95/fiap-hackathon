using FluentAssertions;
using Hackathon.HealthMed.Doctors.Application.Doctors.Create;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;
using NSubstitute;

namespace Hackathon.HealthMed.Doctors.Application.UnitTests.Doctors;

public class CreateDoctorCommandTests
{
    private readonly CreateDoctorCommand Command = new("Test", "test@gmail.com", "73723110045", "123456", "Teste@123", Specialty.Cardiology);

    private readonly IDoctorRepository _doctorRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private readonly CreateDoctorCommandHandler _handler;

    public CreateDoctorCommandTests()
    {
        _doctorRepositoryMock = Substitute.For<IDoctorRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        
        _handler = new CreateDoctorCommandHandler(_doctorRepositoryMock, _unitOfWorkMock);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenNameIsNotValid()
    {
        // Arrange
        var invalidCommand = Command with
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
        var invalidCommand = Command with
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
        var invalidCommand = Command with
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
    public async Task Handle_Should_ReturnError_WhenPassowrdIsNotValid()
    {
        // Arrange
        var invalidCommand = Command with
        {
            Password = "Teste123"
        };
        
        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PasswordErrors.EmptySpecialChar);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenCrmIsInvalid()
    {
        // Arrange
        var invalidCommand = Command with
        {
            Crm = "1234"
        };
        
        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CrmErrors.InvalidFormat);
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
        result.Error.Should().Be(DoctorErrors.CpfNotUnique);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailNotUnique()
    {
        // Arrange
        MockCpf();
        MockEmail(false);
        
        // Act
        Result<Guid> result = await _handler.Handle(Command, default);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DoctorErrors.EmailNotUnique);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenCrmNotUnique()
    {
        // Arrange
        MockCpf();
        MockEmail();
        MockCrm(false);

        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DoctorErrors.CrmNotUnique);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        MockCpf();
        MockEmail();
        MockCrm();

        // Act
        var result = await _handler.Handle(Command, default);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        _doctorRepositoryMock
            .Received(1)
            .Insert(Arg.Any<Doctor>());
        
        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    private void MockCpf(bool isUnique = true)
    {
        _doctorRepositoryMock.IsCpfUniqueAsync(
                Cpf.Create(Command.Cpf).Value, default)
            .Returns(isUnique);
    }
    
    private void MockEmail(bool isUnique = true)
    {
        _doctorRepositoryMock.IsEmailUniqueAsync(
                Email.Create(Command.Email).Value, default)
            .Returns(isUnique);
    }

    private void MockCrm(bool isUnique = true)
    {
        _doctorRepositoryMock.IsCrmUniqueAsync(
                Crm.Create(Command.Crm).Value, default)
            .Returns(isUnique);
    }
}