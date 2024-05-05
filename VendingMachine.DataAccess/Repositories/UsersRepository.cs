using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.Core.Interfaces.Repositories;
using VendingMachine.Core.Models;
using VendingMachine.Core.Models.Enum;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess.Repositories
{
    public class UsersRepository(
        VendingMachineDbContext dbContext,
        IMapper<User, UserEntity> usersMapper) : IUsersRepository
    {
        private readonly VendingMachineDbContext dbContext = dbContext;
        private readonly IMapper<User, UserEntity> usersMapper = usersMapper;

        public async Task<Guid> Add(User user)
        {
            var userEntity = usersMapper.ToEntity(user);

            await dbContext.Users.AddAsync(userEntity);
            await dbContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task<User?> GetById(Guid id)
        {
            var userEntity = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return userEntity != null
                ? usersMapper.ToDomain(userEntity)
                : null;
        }

        public async Task<User?> GetByName(string name)
        {
            var userEntity = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == name);

            return userEntity != null
                ? usersMapper.ToDomain(userEntity)
                : null;
        }

        public async Task<Guid> Update(User user)
        {
            await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Name, user.Name)
                    .SetProperty(u => u.PasswordHash, user.PasswordHash)
                    .SetProperty(u => u.AmountMoney, user.AmountMoney)
                    .SetProperty(u => u.Role, user.Role));

            return user.Id;
        }
    }
}
