using FluentAssertions;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.UnitTest;

public class NameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_ReturnError_WhenNameIsEmpty(string? value)
    {
        // Arrange
        // Act
        Result<Name> name = Name.Create(value);
        
        // Assert
        name.IsFailure.Should().BeTrue();
        name.Error.Should().Be(NameErrors.Empty);
    }
    
    [Fact]
    public void Create_Should_ReturnName_WhenValueIsValid()
    {
        // Arrange
        Result<Name> name = Name.Create("First and Last Name");
        
        // Act
        // Assert
        name.IsSuccess.Should().BeTrue();
        name.Value.Value.Should().Be("First and Last Name");
    }
}