using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Authentication;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class OpmeUserRepository : IOpmeUserRepository
{
    private readonly OpmeContext _context;

    public OpmeUserRepository(OpmeContext context)
    {
        _context = context;
    }

    public async Task Create(OpmeUser user)
    {
        await _context.OpmeUsers.AddAsync(user);
    }

    public async Task<OpmeUser?> FindByEmail(string email)
    {
        return await _context.OpmeUsers.AsNoTracking().SingleOrDefaultAsync(p => p.Email == email);
    }
}
