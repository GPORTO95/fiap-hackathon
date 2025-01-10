using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.HealthMed.Patient.Infrastructure.Data.Configurations;

internal sealed class PatientConfig : IEntityTypeConfiguration<Domain.Patients.Patient>
{
    public void Configure(EntityTypeBuilder<Domain.Patients.Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);
        
        builder.ComplexProperty(
            u => u.Name,
            b => b.Property(e => e.Value).HasColumnName(nameof(Domain.Patients.Patient.Name)));
        
        builder.ComplexProperty(
            u => u.Email,
            b => b.Property(e => e.Value).HasColumnName(nameof(Domain.Patients.Patient.Email)));

        builder.ComplexProperty(
            u => u.Cpf,
            b => b.Property(e => e.Value).HasColumnName(nameof(Domain.Patients.Patient.Cpf)));
        
        builder.ComplexProperty(
            u => u.Password,
            b => b.Property(e => e.Value).HasColumnName("PasswordHash"));
    }
}