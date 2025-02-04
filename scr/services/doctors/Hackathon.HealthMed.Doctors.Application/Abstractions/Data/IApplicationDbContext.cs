using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.HealthMed.Doctors.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Doctor> Doctors { get; set; }
}
