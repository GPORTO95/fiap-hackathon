using Hackathon.HealthMed.Doctors.Domain.Doctors;

namespace Hackathon.HealthMed.Doctors.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string Generate(Doctor doctor);
}