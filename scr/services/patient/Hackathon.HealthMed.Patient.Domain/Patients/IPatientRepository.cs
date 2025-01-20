using Hackathon.HealthMed.Kernel.DomainObjects;

namespace Hackathon.HealthMed.Patient.Domain.Patients;

public interface IPatientRepository
{
    Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default);
    
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    Task<Patient?> LoginAsync(Email email, Password password, CancellationToken cancellationToken = default);
    
    void Insert(Patient patient);
}