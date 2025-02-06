using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.Create;

public sealed record CreateDoctorCommand(
    string Name,
    string Email,
    string Cpf,
    string Crm,
    string Password,
    Specialty Specialty) : ICommand<Guid>;

internal sealed class CreateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateDoctorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return Result.Failure<Guid>(nameResult.Error);
        }
        
        Result<Email> emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }
        
        Result<Cpf> cpfResult = Cpf.Create(request.Cpf);

        if (cpfResult.IsFailure)
        {
            return Result.Failure<Guid>(cpfResult.Error);
        }
        
        Result<Crm> crmResult = Crm.Create(request.Crm);

        if (crmResult.IsFailure)
        {
            return Result.Failure<Guid>(crmResult.Error);
        }
        
        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return Result.Failure<Guid>(passwordResult.Error);
        }

        if (!await doctorRepository.IsCpfUniqueAsync(cpfResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(DoctorErrors.CpfNotUnique);
        }

        if (!await doctorRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(DoctorErrors.EmailNotUnique);
        }

        if (!await doctorRepository.IsCrmUniqueAsync(crmResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(DoctorErrors.CrmNotUnique);
        }

        var doctor = Doctor.Create(
            nameResult.Value, 
            emailResult.Value,
            cpfResult.Value,
            crmResult.Value,
            passwordResult.Value,
             request.Specialty);
        
        doctorRepository.Insert(doctor);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return doctor.Id;
    }
}
