namespace Omini.Opme.Shared.Formatters;

public static class Formatters
{
    public static string GetDigits(this string value)
    {
        return string.Concat(value.Where(Char.IsDigit));
    }

    public static string FormatCpf(string cpf)
    {
        if (cpf is null)
        {
            throw new ArgumentNullException(nameof(cpf));
        }

        var cleanCpf = cpf.GetDigits();

        if (cleanCpf.Length != 11)
        {
            throw new FormatException("Invalid cpf size");
        }

        return $"{cleanCpf[..3]}.{cleanCpf[3..6]}.{cleanCpf[6..9]}-{cleanCpf[9..11]}";
    }

    public static string FormatCnpj(string cnpj)
    {
        if (cnpj is null)
        {
            throw new ArgumentNullException(nameof(cnpj));
        }

        var cleanCpf = cnpj.GetDigits();

        if (cleanCpf.Length != 14)
        {
            throw new FormatException("Invalid cpf size");
        }

        return $"{cleanCpf[..2]}.{cleanCpf[2..5]}.{cleanCpf[5..8]}/{cleanCpf[8..12]}-{cleanCpf[12..14]}";
    }
}