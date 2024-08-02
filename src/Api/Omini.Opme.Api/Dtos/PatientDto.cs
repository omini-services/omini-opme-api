namespace Omini.Opme.Api.Dtos;

public sealed record PatientOutputDto
{
    public string Code { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }
}