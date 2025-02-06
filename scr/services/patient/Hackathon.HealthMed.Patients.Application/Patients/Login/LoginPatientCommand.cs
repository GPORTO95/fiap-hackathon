using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patients.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Patients.Domain.Patients;

namespace Hackathon.HealthMed.Patients.Application.Patients.Login;

public sealed record LoginPatientCommand(string? Email, string? Cpf, string Password) : ICommand<string>;

internal sealed record LoginPatientCommandHandler(
    IPatientRepository patientRepository,
    IJwtProvider jwtProvider) : ICommandHandler<LoginPatientCommand, string>
{
    public async Task<Result<string>> Handle(LoginPatientCommand request, CancellationToken cancellationToken)
    {
        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return Result.Failure<string>(passwordResult.Error);
        }

        Patient? patient = null;
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<string>(emailResult.Error);
            }

            patient = await patientRepository.LoginByEmailAsync(
                emailResult.Value,
                passwordResult.Value,
                cancellationToken);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(request.Cpf))
            {
                Result<Cpf> cpfResult = Cpf.Create(request.Cpf);

                if (cpfResult.IsFailure)
                {
                    return Result.Failure<string>(cpfResult.Error);
                }

                patient = await patientRepository.LoginByCpfAsync(
                    cpfResult.Value,
                    passwordResult.Value,
                    cancellationToken);
            }
        }

        if (patient is null)
        {
            return Result.Failure<string>(PatientErrors.LoginInvalid);
        }

        return jwtProvider.Generate(patient);
    }
}