using Hackathon.HealthMed.Kernel.Data;
using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;
using Hackathon.HealthMed.Patient.Domain.Patients;

namespace Hackathon.HealthMed.Patient.Application.Patients.Create;

internal sealed class CreatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreatePatientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
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
        
        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return Result.Failure<Guid>(passwordResult.Error);
        }

        if (!await patientRepository.IsCpfUniqueAsync(cpfResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(PatientErrors.CpfNotUnique);
        }

        var patient = Domain.Patients.Patient.Create(nameResult.Value, emailResult.Value, cpfResult.Value, passwordResult.Value);
        
        patientRepository.Insert(patient);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Guid.NewGuid();
    }
}