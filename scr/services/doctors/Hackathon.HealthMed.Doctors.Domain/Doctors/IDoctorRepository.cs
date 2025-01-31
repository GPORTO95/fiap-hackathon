using Hackathon.HealthMed.Kernel.DomainObjects;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public interface IDoctorRepository
{
    Task<bool> ExistByIdAsync(Guid doctorId, CancellationToken cancellationToken = default);

    Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default);
    
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    Task<Doctor?> LoginAsync(Email email, Password password, CancellationToken cancellationToken = default);
    
    void Insert(Doctor doctor);
}