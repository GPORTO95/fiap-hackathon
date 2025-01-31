using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Repositories;

internal sealed class DoctorScheduleRepository(ApplicationDbContext context) : IDoctorScheduleRepository
{
    public async Task<bool> ScheduleIsFreeAsync(DateOnly date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken = default)
    {
        var formattedDate = date.ToString("yyyy-MM-dd");
        var formattedStartTime = start.ToString(@"hh\:mm");
        var formattedEndTime = end.ToString(@"hh\:mm");

        const string sql = @"
            SELECT Id
            FROM DoctorSchedules
            WHERE Date = {0}
            AND (START BETWEEN {1} AND {2}
            OR [End] BETWEEN {1} AND {2})";

        return !await context.DoctorSchedules
            .FromSqlRaw(sql, formattedDate, formattedStartTime, formattedEndTime)
            .AnyAsync(cancellationToken);
    }

    public void Add(DoctorSchedule doctorSchedule)
    {
        context.DoctorSchedules.Add(doctorSchedule);
    }
}
