using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patient.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Patient.Domain.Patients;
using Hackathon.HealthMed.Patient.Infrastructure.Authentication;
using Hackathon.HealthMed.Patient.Infrastructure.Data;
using Hackathon.HealthMed.Patient.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.HealthMed.Patient.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // services.AddMediatR(config =>
        //     config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) => options
                .UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IPatientRepository, PatientRepository>();

        services.Configure<JwtOptions>(
            configuration.GetSection("Jwt"));
        services.AddScoped<IJwtProvider, JwtProvider>();
    }
}