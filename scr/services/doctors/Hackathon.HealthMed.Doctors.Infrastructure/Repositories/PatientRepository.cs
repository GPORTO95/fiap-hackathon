using Hackathon.HealthMed.Doctors.Domain.Patients;
using Hackathon.HealthMed.Doctors.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Repositories;

internal sealed class PatientRepository(ApplicationDbContext context) : IPatientRepository
{
    public async Task<bool> ExistByIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await context.Patients.AnyAsync(x => x.Id == patientId, cancellationToken);
    }
}
