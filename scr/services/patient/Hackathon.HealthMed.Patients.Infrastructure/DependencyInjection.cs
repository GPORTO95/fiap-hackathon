using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patients.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Patients.Domain.Patients;
using Hackathon.HealthMed.Patients.Infrastructure.Authentication;
using Hackathon.HealthMed.Patients.Infrastructure.Data;
using Hackathon.HealthMed.Patients.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.HealthMed.Patients.Infrastructure;

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

        services.AddScoped<IPatientRepository, PatientRepository>();
        
        var section = configuration.GetSection("Jwt");
        services.Configure<JwtOptions>(section);
        
        services.AddScoped<IJwtProvider, JwtProvider>();
    }
}