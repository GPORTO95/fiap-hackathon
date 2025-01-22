using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Patients.Domain.Patients;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.HealthMed.Patients.Infrastructure.Data;

internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}