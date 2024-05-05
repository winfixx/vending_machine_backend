using VendingMachine.Core.Models;

namespace VendingMachine.Core.Interfaces.Repositories
{
    public interface IDrinksRepository
    {
        Task<Guid> Add(Drink drinkm, Guid vendingMachineId);
        Task<Drink?> GetById(Guid id);
        Task<IEnumerable<Drink>?> GetAll(); 
        Task<Guid> Update(Drink drink);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<Drink>?> GetAllByVendingId(Guid id);
    }
}
