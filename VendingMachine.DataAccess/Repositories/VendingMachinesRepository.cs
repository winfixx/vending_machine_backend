using Microsoft.EntityFrameworkCore;
using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.Core.Interfaces.Repositories;
using VendingMachine.Core.Models;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess.Repositories
{
    public class VendingMachinesRepository(
        VendingMachineDbContext dbContext,
        IMapper<Core.Models.VendingMachine, VendingMachineEntity> vendingMachinesMapper) : IVendingMachinesRepository
    {
        private readonly VendingMachineDbContext dbContext = dbContext;
        private readonly IMapper<Core.Models.VendingMachine, VendingMachineEntity> vendingMachinesMapper = vendingMachinesMapper;

        public async Task<Guid> Add(Core.Models.VendingMachine vendingMachine)
        {
            var vendingMachineEntity = vendingMachinesMapper.ToEntity(vendingMachine);

            await dbContext.VendingMachine.AddAsync(vendingMachineEntity);
            await dbContext.SaveChangesAsync();

            return vendingMachine.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await dbContext.VendingMachine
                .AsNoTracking()
                .Where(v => v.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<Core.Models.VendingMachine>?> GetAll()
        {
            return await dbContext.VendingMachine
               .AsNoTracking()
               .Select(v => vendingMachinesMapper.ToDomain(v))
               .ToListAsync();
        }

        public async Task<Core.Models.VendingMachine?> GetById(Guid id)
        {
            var vendingMachineEntity = await dbContext.VendingMachine
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            return vendingMachineEntity != null
                ? vendingMachinesMapper.ToDomain(vendingMachineEntity)
                : null;
        }

        public async Task<Guid> Update(Core.Models.VendingMachine vendingMachine)
        {
            await dbContext.VendingMachine
                .AsNoTracking()
                .Where(v => v.Id == vendingMachine.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(v => v.Title, vendingMachine.Title)
                    .SetProperty(v => v.AmountMoney, vendingMachine.AmountMoney));

            return vendingMachine.Id;
        }
    }
}
