namespace Omini.Opme.Be.Api.Dtos;

public sealed record PatientCreateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }
}

public sealed record PatientUpdateDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }
}

public sealed record PatientOutputDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }
}