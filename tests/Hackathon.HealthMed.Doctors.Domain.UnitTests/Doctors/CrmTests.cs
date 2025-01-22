using Hackathon.HealthMed.Doctors.Domain.Doctors;

namespace Hackathon.HealthMed.Doctors.Domain.UnitTests.Doctors;

public class CrmTests
{
    [Fact]
    public void Create_CrmIsNull_ReturnsFailureWithEmptyError()
    {
        // Arrange
        string? crm = null;

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CrmErrors.Empty, result.Error);
    }
    
    [Fact]
    public void Create_CrmIsEmpty_ReturnsFailureWithEmptyError()
    {
        // Arrange
        string crm = "";

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CrmErrors.Empty, result.Error);
    }
    
    [Fact]
    public void Create_CrmIsWhitespace_ReturnsFailureWithEmptyError()
    {
        // Arrange
        string crm = "   ";

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CrmErrors.Empty, result.Error);
    }

    [Fact]
    public void Create_CrmDoesNotMatchRegex_ReturnsFailureWithInvalidFormatError()
    {
        // Arrange
        string crm = "12345"; // Invalid format (less than 6 digits)

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CrmErrors.InvalidFormat, result.Error);
    }

    [Fact]
    public void Create_CrmHasMoreThanSevenDigits_ReturnsFailureWithInvalidFormatError()
    {
        // Arrange
        string crm = "12345678"; // Invalid format (more than 7 digits)

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CrmErrors.InvalidFormat, result.Error);
    }

    [Fact]
    public void Create_CrmIsValid_ReturnsSuccessWithCrmObject()
    {
        // Arrange
        string crm = "123456"; // Valid CRM (6 digits)

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<Crm>(result.Value); // Assuming Crm is the type you expect
        Assert.Equal(crm, result.Value?.Value); // Assuming 'Value' is the property holding the CRM string
    }

    [Fact]
    public void Create_CrmIsValidWithSevenDigits_ReturnsSuccessWithCrmObject()
    {
        // Arrange
        string crm = "1234567"; // Valid CRM (7 digits)

        // Act
        var result = Crm.Create(crm);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<Crm>(result.Value);
        Assert.Equal(crm, result.Value?.Value);
    }
}