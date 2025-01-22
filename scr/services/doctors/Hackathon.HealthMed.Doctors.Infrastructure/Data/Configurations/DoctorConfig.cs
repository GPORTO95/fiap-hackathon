using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.HealthMed.Patients.Infrastructure.Data.Configurations;

internal sealed class DoctorConfig : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");

        builder.HasKey(p => p.Id);
        
        builder.ComplexProperty(
            u => u.Name,
            b => b.Property(e => e.Value).HasColumnName(nameof(Name)));
        
        builder.ComplexProperty(
            u => u.Email,
            b => b.Property(e => e.Value).HasColumnName(nameof(Email)));

        builder.ComplexProperty(
            u => u.Cpf,
            b => b.Property(e => e.Value).HasColumnName(nameof(Cpf)));
        
        builder.ComplexProperty(
            u => u.Crm,
            b => b.Property(e => e.Value).HasColumnName(nameof(Crm)));
        
        builder.ComplexProperty(
            u => u.Password,
            b => b.Property(e => e.Value).HasColumnName("PasswordHash"));
    }
}