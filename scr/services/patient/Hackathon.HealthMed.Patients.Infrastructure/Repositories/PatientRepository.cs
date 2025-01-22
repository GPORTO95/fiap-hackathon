using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Patients.Domain.Patients;
using Hackathon.HealthMed.Patients.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.HealthMed.Patients.Infrastructure.Repositories;

internal sealed class PatientRepository(ApplicationDbContext context) : IPatientRepository
{
    public async Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default)
    {
        return !await context.Patients.AnyAsync(p => p.Cpf == cpf, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return !await context.Patients.AnyAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<Patient?> LoginAsync(Email email, Password password, CancellationToken cancellationToken = default)
    {
        return await context.Patients
            .Where(p => p.Email == email && p.Password == password)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Insert(Patient patient)
    {
        context.Patients.Add(patient);
    }
}