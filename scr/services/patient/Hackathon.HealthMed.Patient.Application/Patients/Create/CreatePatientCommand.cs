using Hackathon.HealthMed.Kernel.Messaging;

namespace Hackathon.HealthMed.Patient.Application.Patients.Create;

public sealed record CreatePatientCommand(
    string Name,
    string Email,
    string Cpf,
    string Password) : ICommand<Guid>;