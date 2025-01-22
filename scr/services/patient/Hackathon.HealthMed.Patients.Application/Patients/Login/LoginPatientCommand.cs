using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patients.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Patients.Domain.Patients;

namespace Hackathon.HealthMed.Patients.Application.Patients.Login;

public sealed record LoginPatientCommand(string Email, string Password) : ICommand<string>;

internal sealed record LoginPatientCommandHandler(
    IPatientRepository patientRepository,
    IJwtProvider jwtProvider) : ICommandHandler<LoginPatientCommand, string>
{
    public async Task<Result<string>> Handle(LoginPatientCommand request, CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<string>(emailResult.Error);
        }
        
        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return Result.Failure<string>(passwordResult.Error);
        }
        
        Patient? patient = await patientRepository.LoginAsync(
            emailResult.Value,
            passwordResult.Value,
            cancellationToken);

        if (patient is null)
        {
            return Result.Failure<string>(PatientErrors.LoginInvalid);
        }

        return jwtProvider.Generate(patient);
    }
}