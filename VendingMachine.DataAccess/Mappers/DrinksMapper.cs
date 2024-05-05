using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.Core.Models;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess.Mappers
{
    public class DrinksMapper : IMapper<Drink, DrinkEntity>
    {
        public Drink ToDomain(DrinkEntity entity)
        {
            var drink = new Drink(
                entity.Id,
                entity.Price,
                entity.Title,
                entity.Image,
                entity.Count);

            return drink;
        }

        public DrinkEntity ToEntity(Drink domainModel)
        {
            var drinkEntity = new DrinkEntity
            {
                Id = domainModel.Id,
                Price = domainModel.Price,
                Title = domainModel.Title,
                Image = domainModel.Image,
                Count = domainModel.Count
            };

            return drinkEntity;
        }
    }
}
