using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.DomainObjects;

public sealed record Name
{
    private Name(string value) => Value = value;
    public string Value { get; }

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Name>(NameErrors.Empty);
        }

        return new Name(name);
    }
}

public static class NameErrors
{
    public static readonly Error Empty = Error.Problem("Name.Empty", "Name is empty");
}