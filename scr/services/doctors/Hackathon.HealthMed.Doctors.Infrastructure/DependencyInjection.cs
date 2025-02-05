using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Infrastructure.Authentication;
using Hackathon.HealthMed.Doctors.Infrastructure.Data;
using Hackathon.HealthMed.Doctors.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Data;
using Hackathon.HealthMed.Doctors.Domain.Appointments;
using Hackathon.HealthMed.Doctors.Domain.Patients;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Notifications;
using Hackathon.HealthMed.Doctors.Infrastructure.Services.Notifications;

namespace Hackathon.HealthMed.Doctors.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) => options
                .UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        var section = configuration.GetSection("Jwt");
        services.Configure<JwtOptions>(section);

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<INotificationService, NotificationService>();
    }
}