using Hackathon.HealthMed.Kernel.DomainObjects;

namespace Hackathon.HealthMed.Patients.Domain.Patients;

public interface IPatientRepository
{
    Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default);
    
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    Task<Patient?> LoginByEmailAsync(Email email, Password password, CancellationToken cancellationToken = default);

    Task<Patient?> LoginByCpfAsync(Cpf cpf, Password password, CancellationToken cancellationToken = default);

    void Insert(Patient patient);
}