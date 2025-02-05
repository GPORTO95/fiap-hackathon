using Hackathon.HealthMed.Doctors.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Data.Configurations;

internal sealed class AppointmentConfig : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(x => x.Id);
    }
}
