using System.Text.RegularExpressions;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.doctors;

public sealed record Crm
{
    private Crm(string value) => Value = value;
    
    public string Value { get; set; }

    public static Result<Crm> Create(string? crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
        {
            return Result.Failure<Crm>(CrmErrors.Empty);
        }

        if (!Regex.IsMatch(crm, @"^\d{6,7}$"))
        {
            return Result.Failure<Crm>(CrmErrors.InvalidFormat);
        }

        return new Crm(crm);
    }
}