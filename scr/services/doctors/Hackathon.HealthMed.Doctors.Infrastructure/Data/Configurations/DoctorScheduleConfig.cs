using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.HealthMed.Patients.Infrastructure.Data.Configurations;

internal sealed class DoctorScheduleConfig : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedules");
        
        builder.HasKey(p => p.Id);
        
        builder.OwnsOne(e => e.Time, ts =>
        {
            ts.Property(tr => tr.Date)
                .HasColumnName("Date");

            ts.Property(tr => tr.Start)
                .HasColumnName("Start");

            ts.Property(tr => tr.End)
                .HasColumnName("End");
        });
    }
}