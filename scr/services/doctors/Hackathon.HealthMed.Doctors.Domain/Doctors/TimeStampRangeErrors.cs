using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public static class TimeStampRangeErrors
{
    public static readonly Error DateInvalid = Error.Problem(
        "TimeRange.DateInvalid", 
        "Date should be greater than or equal to the current date");
    
    public static readonly Error StartInvalid = Error.Problem(
        "TimeRange.StartInvalid", 
        "Start time should be less than end time.");

    public static readonly Error TimeOutOfRange = Error.Problem(
        "TimeRange.TimeOutOfRange",
        "O horário deve estar entre 00:01 e 24:59.");
}

