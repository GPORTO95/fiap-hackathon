using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public static class CrmErrors
{
    public static readonly Error Empty = Error.Problem("Crm.Empty", "Crm is empty");

    public static readonly Error InvalidFormat = Error.Problem("Crm.InvalidFormat", "Crm format is invalid");
}