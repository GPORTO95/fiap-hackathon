namespace Hackathon.HealthMed.Patient.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string Generate(Domain.Patients.Patient patient);
}