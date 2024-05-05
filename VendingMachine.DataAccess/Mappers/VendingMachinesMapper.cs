using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess.Mappers
{
    public class VendingMachinesMapper : IMapper<Core.Models.VendingMachine, VendingMachineEntity>
    {
        public Core.Models.VendingMachine ToDomain(VendingMachineEntity entity)
        {
            var vendingMachine = new Core.Models.VendingMachine(
                entity.Id,
                entity.Title,
                entity.AmountMoney);
            
            return vendingMachine;
        }

        public VendingMachineEntity ToEntity(Core.Models.VendingMachine domainModel)
        {
            var vendingMachineEntity = new VendingMachineEntity
            {
                Id = domainModel.Id,
                Title = domainModel.Title,
                AmountMoney = domainModel.AmountMoney,
            };

            return vendingMachineEntity;
        }
    }
}
