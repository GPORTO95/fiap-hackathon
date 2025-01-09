using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Kernel.DomainObjects;

public sealed record Cpf
{
    public const int CpfMaxLengt = 11;
    
    private Cpf(string value) => Value = value;
    
    public string Value { get; }

    public static Result<Cpf> Create(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return Result.Failure<Cpf>(CpfErrors.Empty); 
        }
        
        if (cpf.Length != 11)
            return Result.Failure<Cpf>(CpfErrors.InvalidLength);

        var igual = true;
        for (var i = 1; i < 11 && igual; i++)
            if (cpf[i] != cpf[0])
                igual = false;

        if (igual || cpf == "12345678909")
            return Result.Failure<Cpf>(CpfErrors.InvalidFormat);

        var numeros = new int[11];

        for (var i = 0; i < 11; i++)
            numeros[i] = int.Parse(cpf[i].ToString());

        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += (10 - i) * numeros[i];

        var resultado = soma % 11;

        if (resultado == 1 || resultado == 0)
        {
            if (numeros[9] != 0)
                return Result.Failure<Cpf>(CpfErrors.InvalidFormat);
        }
        else if (numeros[9] != 11 - resultado)
            return Result.Failure<Cpf>(CpfErrors.InvalidFormat);

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += (11 - i) * numeros[i];

        resultado = soma % 11;

        if (resultado == 1 || resultado == 0)
        {
            if (numeros[10] != 0)
                return Result.Failure<Cpf>(CpfErrors.InvalidFormat);
        }
        else if (numeros[10] != 11 - resultado)
            return Result.Failure<Cpf>(CpfErrors.InvalidFormat);

        return new Cpf(cpf);
    }
}

public static class CpfErrors
{
    public static readonly Error Empty = Error.Problem("Cpf.Empty", "Cpf is empty");
    
    public static readonly Error InvalidLength = Error.Problem("Cpf.InvalidLength", "Cpf length is invalid");

    public static readonly Error InvalidFormat = Error.Problem("Cpf.InvalidFormat", "Cpf format is invalid");
}