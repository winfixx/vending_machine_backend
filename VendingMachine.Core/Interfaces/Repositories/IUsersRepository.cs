using VendingMachine.Core.Models;
using VendingMachine.Core.Models.Enum;

namespace VendingMachine.Core.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<Guid> Add(User user);
        Task<User?> GetByName(string name);
        Task<User?> GetById(Guid id);
        Task<Guid> Update(User user);
    }
}