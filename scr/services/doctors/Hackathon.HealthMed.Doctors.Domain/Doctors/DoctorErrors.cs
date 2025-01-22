using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public static class DoctorErrors
{
    public static Error CpfNotUnique = Error.Conflict(
        "Doctor.CpfNotUnique",
        "The provided cpf is not unique");
    
    public static Error EmailNotUnique = Error.Conflict(
        "Doctor.EmailNotUnique",
        "The provided email is not unique");
    
    public static readonly Error LoginInvalid = Error.Problem(
        "Doctor.LoginInvalid", 
        "Email or password informed is invalid");
}