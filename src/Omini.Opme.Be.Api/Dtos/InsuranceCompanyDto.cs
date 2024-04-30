namespace Omini.Opme.Be.Api.Dtos;

public sealed record InsuranceCompanyCreateDto
{
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }
}

public sealed record InsuranceCompanyUpdateDto
{
    public Guid Id { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }
}

public sealed record InsuranceCompanyOutputDto
{
    public Guid Id { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
}