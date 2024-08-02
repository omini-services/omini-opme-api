namespace Omini.Opme.Api.Dtos;

public sealed record PhysicianOutputDto
{
    public string Code { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }

    public string Comments { get; set; }
}