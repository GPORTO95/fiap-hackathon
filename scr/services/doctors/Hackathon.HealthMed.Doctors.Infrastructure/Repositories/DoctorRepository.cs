using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Doctors.Infrastructure.Data;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.HealthMed.Doctors.Infrastructure.Repositories;

internal sealed class DoctorRepository(ApplicationDbContext context) : IDoctorRepository
{
    public async Task<bool> ExistByIdAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return await context.Doctors.AnyAsync(d => d.Id == doctorId, cancellationToken);
    }

    public async Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default)
    {
        return !await context.Doctors.AnyAsync(p => p.Cpf == cpf, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return !await context.Doctors.AnyAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<Doctor?> LoginAsync(Email email, Password password, CancellationToken cancellationToken = default)
    {
        return await context.Doctors
            .Where(p => p.Email == email && p.Password == password)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Insert(Doctor doctor)
    {
        context.Doctors.Add(doctor);
    }
}