namespace Hackathon.HealthMed.Doctors.Application.Abstractions.Notifications;

public interface INotificationService
{
    Task SendEmailAsync(CancellationToken cancellationToken = default);
}
