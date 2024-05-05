using VendingMachine.Core.Models;

namespace VendingMachine.Core.Interfaces.Repositories
{
    public interface IVendingMachinesRepository
    {
        Task<Guid> Add(Models.VendingMachine vendingMachine);
        Task<Models.VendingMachine?> GetById(Guid id);
        Task<IEnumerable<Models.VendingMachine>?> GetAll();
        Task<Guid> Update(Models.VendingMachine vendingMachine);
        Task<Guid> Delete(Guid id);
    }
}
