using Hackathon.HealthMed.Kernel.DomainObjects;

namespace Hackathon.HealthMed.Patient.Domain.Patients;

public interface IPatientRepository
{
    Task<bool> IsCpfUniqueAsync(Cpf cpf, CancellationToken cancellationToken = default);

    void Insert(Patient patient);
}