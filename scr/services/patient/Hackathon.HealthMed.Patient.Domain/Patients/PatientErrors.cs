using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Patient.Domain.Patients;

public static class PatientErrors
{
    public static Error CpfNotUnique = Error.Conflict(
        "Patients.CpfNotUnique",
        "The provided cpf is not unique");
}