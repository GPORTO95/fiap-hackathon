namespace Hackathon.HealthMed.Patients.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string Generate(Domain.Patients.Patient patient);
}