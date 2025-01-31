using Hackathon.HealthMed.Doctors.Domain.Doctors;

namespace Hackathon.HealthMed.Doctors.Domain.UnitTests.Doctors;

public class TimeStampRangeTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)); // Data futura
        var startTime = new TimeSpan(10, 0, 0); // 10:00:00
        var endTime = new TimeSpan(12, 0, 0);   // 12:00:00

        // Act
        var result = TimeStampRange.Create(date, startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(date, result.Value.Date);
        Assert.Equal(startTime, result.Value.Start);
        Assert.Equal(endTime, result.Value.End);
    }

    [Fact]
    public void Create_WithPastDate_ShouldReturnFailure()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)); // Data passada
        var startTime = new TimeSpan(10, 0, 0); // 10:00:00
        var endTime = new TimeSpan(12, 0, 0);   // 12:00:00

        // Act
        var result = TimeStampRange.Create(date, startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeStampRangeErrors.DateInvalid, result.Error);
    }

    [Fact]
    public void Create_WithStartTimeGreaterThanEndTime_ShouldReturnFailure()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)); // Data futura
        var startTime = new TimeSpan(12, 0, 0); // 12:00:00
        var endTime = new TimeSpan(10, 0, 0);   // 10:00:00

        // Act
        var result = TimeStampRange.Create(date, startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeStampRangeErrors.StartInvalid, result.Error);
    }

    [Fact]
    public void Create_WithStartTimeBeforeMinTime_ShouldReturnFailure()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)); // Data futura
        var startTime = new TimeSpan(0, 0, 0); // 00:00:00 (fora do intervalo válido)
        var endTime = new TimeSpan(12, 0, 0);  // 12:00:00

        // Act
        var result = TimeStampRange.Create(date, startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeStampRangeErrors.TimeOutOfRange, result.Error);
    }

    [Fact]
    public void Create_WithEndTimeAfterMaxTime_ShouldReturnFailure()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)); // Data futura
        var startTime = new TimeSpan(10, 0, 0); // 10:00:00
        var endTime = new TimeSpan(25, 0, 0);   // 25:00:00 (fora do intervalo válido)

        // Act
        var result = TimeStampRange.Create(date, startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeStampRangeErrors.TimeOutOfRange, result.Error);
    }

    [Fact]
    public void Create_WithCurrentDateAndValidTimes_ShouldReturnSuccess()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now); // Data atual
        var startTime = new TimeSpan(10, 0, 0); // 10:00:00
        var endTime = new TimeSpan(12, 0, 0);   // 12:00:00

        // Act
        var result = TimeStampRange.Create(date, startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(date, result.Value.Date);
        Assert.Equal(startTime, result.Value.Start);
        Assert.Equal(endTime, result.Value.End);
    }
}
