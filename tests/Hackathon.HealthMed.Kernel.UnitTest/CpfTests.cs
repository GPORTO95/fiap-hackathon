using FluentAssertions;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.UnitTest;

public class CpfTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_ReturnError_WhenCpfIsEmpty(string? value)
    {
        // Arrange
        // Act
        Result<Cpf> cpf = Cpf.Create(value);
        
        // Assert
        cpf.IsFailure.Should().BeTrue();
        cpf.Error.Should().Be(CpfErrors.Empty);
    }
    
    [Theory]
    [InlineData("12315")]
    [InlineData("132113212313")]
    public void Create_Should_ReturnError_WhenCpfLengthIsInvalid(string? value)
    {
        // Arrange
        // Act
        Result<Cpf> cpf = Cpf.Create(value);
        
        // Assert
        cpf.IsFailure.Should().BeTrue();
        cpf.Error.Should().Be(CpfErrors.InvalidLength);
    }
    
    [Theory]
    [InlineData("11111111111")]
    [InlineData("99999999999")]
    public void Create_Should_ReturnError_WhenCpfIsInvalid(string? value)
    {
        // Arrange
        // Act
        Result<Cpf> cpf = Cpf.Create(value);
        
        // Assert
        cpf.IsFailure.Should().BeTrue();
        cpf.Error.Should().Be(CpfErrors.InvalidFormat);
    }
    
    [Fact]
    public void Create_Should_ReturnCpf_WhenValueIsValid()
    {
        // Arrange
        Result<Cpf> cpf = Cpf.Create("98299530091");
        
        // Act
        // Assert
        cpf.IsSuccess.Should().BeTrue();
        cpf.Value.Value.Should().Be("98299530091");
    }
}