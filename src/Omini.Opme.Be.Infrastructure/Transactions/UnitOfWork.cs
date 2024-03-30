using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Transaction;

internal class UnitOfWork : IUnitOfWork
{
    private readonly OpmeContext _diContext;
    public UnitOfWork(OpmeContext diContext)
    {
        _diContext = diContext;
    }

    public async Task Commit()
    {
        var a = await _diContext.SaveChangesAsync();
    }
}