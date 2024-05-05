using Microsoft.EntityFrameworkCore;
using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.Core.Interfaces.Repositories;
using VendingMachine.Core.Models;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess.Repositories
{
    public class DrinksRepository(
        VendingMachineDbContext dbContext,
        IMapper<Drink, DrinkEntity> drinksMapper) : IDrinksRepository
    {
        private readonly VendingMachineDbContext dbContext = dbContext;
        private readonly IMapper<Drink, DrinkEntity> drinksMapper = drinksMapper;

        public async Task<Guid> Add(Drink drink, Guid vendingMachineId)
        {
            var drinkEntity = drinksMapper.ToEntity(drink);
            drinkEntity.VendingMachineId = vendingMachineId;

            await dbContext.Drinks.AddAsync(drinkEntity);
            await dbContext.SaveChangesAsync();

            return drink.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await dbContext.Drinks
                .AsNoTracking()
                .Where(d => d.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<Drink>?> GetAll()
        {
            return await dbContext.Drinks
                .AsNoTracking()
                .Select(d => drinksMapper.ToDomain(d))
                .ToListAsync();
        }

        public async Task<Drink?> GetById(Guid id)
        {
            var drinkEntity = await dbContext.Drinks
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            return drinkEntity != null
                ? drinksMapper.ToDomain(drinkEntity)
                : null;
        }

        public async Task<IEnumerable<Drink>?> GetAllByVendingId(Guid id)
        {
            return await dbContext.Drinks
                .AsNoTracking()
                .Where(d => d.VendingMachineId == id)
                .Select(d => drinksMapper.ToDomain(d))
                .ToListAsync();
        }

        public async Task<Guid> Update(Drink drink)
        {
            await dbContext.Drinks
                 .AsNoTracking()
                 .Where(d => d.Id == drink.Id)
                 .ExecuteUpdateAsync(s => s
                    .SetProperty(d => d.Title, drink.Title)
                    .SetProperty(d => d.Price, drink.Price)
                    .SetProperty(d => d.Image, drink.Image)
                    .SetProperty(d => d.Count, drink.Count));

            return drink.Id;
        }
    }
}
