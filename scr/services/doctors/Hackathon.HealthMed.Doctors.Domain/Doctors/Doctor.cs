using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public sealed class Doctor : Entity
{
    public Doctor()
    { }
    
    public Doctor(Guid id, Name name, Email email, Cpf cpf, Crm crm, Password password) : base(id)
    {
        Name = name;
        Email = email;
        Cpf = cpf;
        Crm = crm;
        Password = password;
    }

    public Name Name { get; set; }
    public Email Email { get; set; }
    public Cpf Cpf { get; set; }
    public Crm Crm { get; set; }
    public Password Password { get; set; }

    public static Doctor Create(Name name, Email email, Cpf cpf, Crm crm, Password password)
    {
        return new Doctor(Guid.NewGuid(), name, email, cpf, crm, password);
    }
}