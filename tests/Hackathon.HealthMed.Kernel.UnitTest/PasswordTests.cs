using FluentAssertions;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.UnitTest;

public class PasswordTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_ReturnError_WhenPasswordIsEmpty(string? value)
    {
        // Arrange
        // Act
        Result<Password> name = Password.Create(value);
        
        // Assert
        name.IsFailure.Should().BeTrue();
        name.Error.Should().Be(PasswordErrors.Empty);
    }
    
    [Fact]
    public void Create_Should_ReturnError_WhenPasswordIsEmptyUpperCase()
    {
        // Arrange
        // Act
        Result<Password> name = Password.Create("password");
        
        // Assert
        name.IsFailure.Should().BeTrue();
        name.Error.Should().Be(PasswordErrors.EmptyUpperCase);
    }
    
    [Fact]
    public void Create_Should_ReturnError_WhenPasswordIsEmptyLowerCase()
    {
        // Arrange
        // Act
        Result<Password> name = Password.Create("PASSWORD");
        
        // Assert
        name.IsFailure.Should().BeTrue();
        name.Error.Should().Be(PasswordErrors.EmptyLowerCase);
    }
    
    [Fact]
    public void Create_Should_ReturnError_WhenPasswordIsEmptyNumber()
    {
        // Arrange
        // Act
        Result<Password> name = Password.Create("PassWord");
        
        // Assert
        name.IsFailure.Should().BeTrue();
        name.Error.Should().Be(PasswordErrors.EmptyNumber);
    }
    
    [Fact]
    public void Create_Should_ReturnError_WhenPasswordIsEmptyEspecialChar()
    {
        // Arrange
        // Act
        Result<Password> name = Password.Create("PassWord1");
        
        // Assert
        name.IsFailure.Should().BeTrue();
        name.Error.Should().Be(PasswordErrors.EmptySpecialChar);
    }
    
    [Fact]
    public void Create_Should_ReturnPassword_WhenValueIsValid()
    {
        // Arrange
        // Act
        Result<Password> name = Password.Create("Pass1Word*");
        
        // Assert
        name.IsSuccess.Should().BeTrue();
        name.Value.Value.Should().Be("Pass1Word*");
    }
}