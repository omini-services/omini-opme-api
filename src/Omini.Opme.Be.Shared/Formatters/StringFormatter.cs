using System.Text.RegularExpressions;

namespace Omini.Opme.Be.Shared;

public static class Formatters
{
    public static string GetNumbersOnly(this string value)
    {
        return Regex.Match(value, @"\d+").Value;
    }

    public static string FormatCpf(string cpf)
    {
        if (cpf is null)
        {
            throw new ArgumentNullException(nameof(cpf));
        }

        var cleanCpf = cpf.GetNumbersOnly();

        if (cleanCpf.Length != 11)
        {
            throw new FormatException("Invalid cpf size");
        }

        return $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..11]}";
    }

    public static string FormatCnpj(string cnpj)
    {
        if (cnpj is null)
        {
            throw new ArgumentNullException(nameof(cnpj));
        }

        var cleanCpf = cnpj.GetNumbersOnly();

        if (cleanCpf.Length != 14)
        {
            throw new FormatException("Invalid cpf size");
        }

        return $"{cnpj[..2]}.{cnpj[2..5]}.{cnpj[5..8]}/{cnpj[8..12]}-{cnpj[12..14]}";
    }
}