namespace Omini.Opme.Be.Api.Dtos;

public sealed record PhysicianCreateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }
}

public sealed record PhysicianUpdateDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }

    public string Comments { get; set; }
}

public sealed record PhysicianOutputDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }

    public string Comments { get; set; }
}