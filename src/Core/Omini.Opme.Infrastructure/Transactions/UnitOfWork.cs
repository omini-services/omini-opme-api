using Omini.Opme.Domain.Transactions;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Transaction;

internal class UnitOfWork : IUnitOfWork
{
    private readonly OpmeContext _opmeContext;
    public UnitOfWork(OpmeContext opmeContext)
    {
        _opmeContext = opmeContext;
    }

    public async Task Commit(CancellationToken cancellationToken = default)
    {
        await _opmeContext.SaveChangesAsync(cancellationToken);
    }
}