using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Patients;

public static class PatientErrors
{
    public static Error NotFound = Error.NotFound(
        "Patient.NotFound",
        "Patient not found");
}