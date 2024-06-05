namespace Omini.Opme.Api.Dtos;


public sealed record InsuranceCompanyOutputDto
{
    public string Code { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }
}