using Hackathon.HealthMed.Doctors.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Data;
using Hackathon.HealthMed.Doctors.Application.Abstractions.Notifications;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Domain.Patients;
using Hackathon.HealthMed.Doctors.Infrastructure.Authentication;
using Hackathon.HealthMed.Doctors.Infrastructure.Data;
using Hackathon.HealthMed.Doctors.Infrastructure.Repositories;
using Hackathon.HealthMed.Doctors.Infrastructure.Services.Notifications;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hackathon.HealthMed.Doctors.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
    {

        var logger = loggerFactory.CreateLogger("Test");


        string? connectionString = "Server={MSSQL_HOST};Database=PublicEnterpriseDb;User ID={MSSQL_USER};Password={SA_PASSWORD};Encrypt=True;TrustServerCertificate=True;";
        Ensure.NotNullOrEmpty(connectionString);


        var sqlUser = Environment.GetEnvironmentVariable("MSSQL_USER") ?? "sa";

        var saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "sqlpassword!123";

        var host = Environment.GetEnvironmentVariable("MSSQL_HOST") ?? "host.docker.internal,1433";

        connectionString = connectionString.Replace("{MSSQL_USER}", sqlUser)
                                           .Replace("{SA_PASSWORD}", saPassword)
                                           .Replace("{MSSQL_HOST}", host);


        logger.LogInformation("TestConnection");
        logger.LogInformation(connectionString);

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) => options
                .UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());


        var jwtOptions = new JwtOptions
        {
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "HealthMedPatient",
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "HealthMedPatient",
            SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "super-secret-key-value-dbb6504e-133d-4654-bf55-15536b019434!",     
        };

        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = jwtOptions.Issuer;
            options.Audience = jwtOptions.Audience;
            options.SecretKey = jwtOptions.SecretKey;
        });

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<INotificationService, NotificationService>();

        AddJwtAuthentication(services, configuration);
    }

    private static void AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? configuration["Jwt:Issuer"];
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? configuration["Jwt:Audience"];
        var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? configuration["Jwt:SecretKey"];

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "scheme-doctor";
            })
            .AddJwtBearer("scheme-doctor", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey!)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddJwtBearer("scheme-patient", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services
            .AddAuthorizationBuilder()
            .AddPolicy("DoctorOnly", policy => policy.RequireClaim("profile", "doctor"))
            .AddPolicy("PatientOnly", policy => policy.RequireClaim("profile", "patient"));
    }
}