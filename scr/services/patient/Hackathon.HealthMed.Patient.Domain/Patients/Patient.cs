using Hackathon.HealthMed.Kernel.DomainObjects;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Patient.Domain.Patients;

public sealed class Patient : Entity
{
    protected Patient()
    { }
    
    private Patient(Guid id, Name name, Email email, Cpf cpf, Password password) : base(id)
    {
        Name = name;
        Email = email;
        Cpf = cpf;
        Password = password;
    }

    public Name Name { get; set; }
    public Email Email { get; set; }
    public Cpf Cpf { get; set; }
    public Password Password { get; set; }

    public static Patient Create(Name name, Email email, Cpf cpf, Password password)
    {
        return new Patient(Guid.NewGuid(), name, email, cpf, password);
    }
}