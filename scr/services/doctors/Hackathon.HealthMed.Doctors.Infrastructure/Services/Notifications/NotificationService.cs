using Hackathon.HealthMed.Doctors.Application.Abstractions.Notifications;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Services.Notifications;

internal sealed class NotificationService : INotificationService
{
    public Task SendEmailAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
