using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.Core.Models;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess.Mappers
{
    public class UsersMapper : IMapper<User, UserEntity>
    {
        public User ToDomain(UserEntity entity)
        {
            var user = new User(entity.Id, entity.Name, entity.PasswordHash, entity.AmountMoney, entity.Role);
            return user;
        }

        public UserEntity ToEntity(User domainModel)
        {
            var userEntity = new UserEntity
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                PasswordHash = domainModel.PasswordHash,
                AmountMoney = domainModel.AmountMoney,
                Role = domainModel.Role
            };

            return userEntity;
        }
    }
}
