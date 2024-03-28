namespace Omini.Opme.Be.Domain.Transactions;

public interface IUnitOfWork
{
    Task Commit();
}