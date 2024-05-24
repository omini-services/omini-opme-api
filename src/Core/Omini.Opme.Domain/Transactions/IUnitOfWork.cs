namespace Omini.Opme.Domain.Transactions;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken = default);
}