using Omini.Opme.Domain.Authentication;

namespace Omini.Opme.Domain.Repositories;

public interface IOpmeUserRepository
{
    Task Create(OpmeUser user);
    Task<OpmeUser?> FindByEmail(string email);
}