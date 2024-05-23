namespace Omini.Opme.Api.Dtos;

public sealed record HospitalOutputDto
{
    public Guid Id { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }
}