using FluentAssertions;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.UnitTest;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_ReturnError_WhenEmailIsEmpty(string? value)
    {
        // Arrange
        // Act
        Result<Email> email = Email.Create(value);
        
        // Assert
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(EmailErrors.Empty);
    }
    
    [Theory]
    [InlineData("teste")]
    [InlineData("teste@")]
    [InlineData("teste@teste")]
    public void Create_Should_ReturnError_WhenEmailIsInvalid(string? value)
    {
        // Arrange
        // Act
        Result<Email> email = Email.Create(value);
        
        // Assert
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(EmailErrors.InvalidFormat);
    }
    
    [Fact]
    public void Create_Should_ReturnEmail_WhenValueIsValid()
    {
        // Arrange
        Result<Email> email = Email.Create("teste@teste.com");
        
        // Act
        // Assert
        email.IsSuccess.Should().BeTrue();
        email.Value.Value.Should().Be("teste@teste.com");
    }
}