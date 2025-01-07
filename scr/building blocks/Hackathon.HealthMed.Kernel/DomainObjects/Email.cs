using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.DomainObjects;

public sealed record Email
{
    private Email(string value) => Value = value;
    
    public string Value { get; set; }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(EmailErrors.Empty);
        }

        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        return new Email(email);
    }
}

public static class EmailErrors
{
    public static readonly Error Empty = Error.Problem("Email.Empty", "Email is empty");

    public static readonly Error InvalidFormat = Error.Problem("Email.InvalidFormat", "Email format is invalid");
}
