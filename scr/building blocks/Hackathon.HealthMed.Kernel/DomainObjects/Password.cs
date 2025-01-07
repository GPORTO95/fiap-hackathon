using System.Text.RegularExpressions;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.DomainObjects;

public sealed record Password
{
    private Password(string value) => Value = value;
    
    public string Value { get; }

    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure<Password>(PasswordErrors.Empty);
        }
        
        if (!Regex.IsMatch(@password, @"[A-Z]"))
        {
            return Result.Failure<Password>(PasswordErrors.EmptyUpperCase);
        }
        
        if (!Regex.IsMatch(@password, @"[a-z]"))
        {
            return Result.Failure<Password>(PasswordErrors.EmptyLowerCase);
        }
        
        if (!Regex.IsMatch(@password, @"[\W_]"))
        {
            return Result.Failure<Password>(PasswordErrors.EmptySpecialChar);
        }
        
        return new Password(password);
    }
}

public static class PasswordErrors
{
    public static readonly Error Empty = Error.Problem("Password.Empty", "Password is empty");

    public static readonly Error EmptyUpperCase = Error.Problem("Password.EmptyUpperCase", "Password uppercase is empty");
    
    public static readonly Error EmptyLowerCase = Error.Problem("Password.EmptyLowerCase", "Password lowercase is empty");
    
    public static readonly Error EmptySpecialChar = Error.Problem("Password.EmptySpecialChar", "Password especial char is empty");
}
