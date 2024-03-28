namespace Omini.Opme.Be.Domain.Repositories;

public interface IItemRepository
{
    Task Create(Item item);
    Task<Item> GetById(Guid id);
    Task<IList<Item>> GetAll();  
    void Update(Item item);
}