namespace Hackathon.HealthMed.Kernel.Shared;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}