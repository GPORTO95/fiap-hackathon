using Hackathon.HealthMed.Doctors.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Data.Configurations;

internal sealed class PatientConfig : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(x => x.Id);
    }
}
