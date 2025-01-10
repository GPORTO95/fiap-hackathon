using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Patient.Domain.Patients;
using Hackathon.HealthMed.Patient.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.HealthMed.Patient.Infrastructure.Repositories;

internal sealed class PatientRepository(ApplicationDbContext context) : IPatientRepository
{
    public async Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default)
    {
        return !await context.Patients.AnyAsync(p => p.Cpf == cpf, cancellationToken);
    }

    public void Insert(Domain.Patients.Patient patient)
    {
        context.Patients.Add(patient);
    }
}