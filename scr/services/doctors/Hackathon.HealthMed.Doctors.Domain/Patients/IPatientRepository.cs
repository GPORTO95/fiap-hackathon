namespace Hackathon.HealthMed.Doctors.Domain.Patients;

public interface IPatientRepository
{
    Task<bool> ExistByIdAsync(Guid patientId, CancellationToken cancellationToken = default);
}
