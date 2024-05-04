using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Domain.Repositories;

public interface IIdentityOpmeUserRepository
{
    Task Create(IdentityOpmeUser user);
    Task<IdentityOpmeUser?> FindByEmail(string email);
}