using Hackathon.HealthMed.Doctors.Application.Abstractions.Authentication;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.Login;

public sealed record LoginDoctorCommand(
    string Email,
    string Password) : ICommand<string>;
    
internal sealed record LoginDoctorCommandHandler(
    IDoctorRepository DoctorRepository,
    IJwtProvider jwtProvider) : ICommandHandler<LoginDoctorCommand, string>
{
    public async Task<Result<string>> Handle(LoginDoctorCommand request, CancellationToken cancellationToken)
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
        
        Doctor? doctor = await DoctorRepository.LoginAsync(
            emailResult.Value,
            passwordResult.Value,
            cancellationToken);

        if (doctor is null)
        {
            return Result.Failure<string>(DoctorErrors.LoginInvalid);
        }

        return jwtProvider.Generate(doctor);
    }
}    
