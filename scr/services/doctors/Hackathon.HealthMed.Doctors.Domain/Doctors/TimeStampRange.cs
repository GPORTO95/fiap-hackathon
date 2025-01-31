using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public sealed record TimeStampRange
{
    private TimeStampRange(DateOnly date, TimeSpan start, TimeSpan end)
    {
        Date = date;
        Start = start;
        End = end;
    }
    
    public DateOnly Date { get; }
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    
    public static Result<TimeStampRange> Create(DateOnly date, TimeSpan startTime, TimeSpan endTime)
    {
        if (date < DateOnly.FromDateTime(DateTime.Now.Date))
        {
            return Result.Failure<TimeStampRange>(TimeStampRangeErrors.DateInvalid);
        }

        if (startTime > endTime)
        {
            return Result.Failure<TimeStampRange>(TimeStampRangeErrors.StartInvalid);
        }

        var minTime = new TimeSpan(0, 1, 0); // 00:01:00
        var maxTime = new TimeSpan(24, 59, 0); // 24:59:00

        if (startTime < minTime || startTime > maxTime || endTime < minTime || endTime > maxTime)
        {
            return Result.Failure<TimeStampRange>(TimeStampRangeErrors.TimeOutOfRange);
        }

        return new TimeStampRange(date, startTime, endTime);
    }
}

