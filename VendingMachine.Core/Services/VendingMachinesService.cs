using VendingMachine.Core.Exceptions;
using VendingMachine.Core.Interfaces.Repositories;

namespace VendingMachine.Core.Services
{
    public class VendingMachinesService(IVendingMachinesRepository vendingMachinesRepository)
    {
        private readonly IVendingMachinesRepository vendingMachinesRepository = vendingMachinesRepository;

        public async Task<Guid> Add(
            string title,
            decimal? amountMoney)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Недостаток данных");
            if (amountMoney < -1)
                throw new ArgumentException("Сумма денег не может быть меньше нуля");

            var vendingMachine = new Models.VendingMachine(
                Guid.NewGuid(),
                title,
                amountMoney == -1 ? 0 : amountMoney.Value);

            await vendingMachinesRepository.Add(vendingMachine);

            return vendingMachine.Id;
        }

        public async Task<Guid> Update(
            Guid id,
            string? title,
            decimal? amountMoney)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            var vendingMachine = await GetById(id);

            await Update(title, amountMoney, vendingMachine);

            return vendingMachine.Id;
        }

        public async Task<Guid> Update(
            string? title,
            decimal? amountMoney,
            Models.VendingMachine vendingMachine)
        {
            if (amountMoney < -1)
                throw new ArgumentException("Сумма денег не может быть меньше нуля");

            title = string.IsNullOrEmpty(title) ? vendingMachine.Title : title;
            amountMoney = amountMoney == -1 ? vendingMachine.AmountMoney : amountMoney;

            await vendingMachinesRepository.Update(new(
                vendingMachine.Id,
                title,
                amountMoney.Value));

            return vendingMachine.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            _ = await GetById(id);

            await vendingMachinesRepository.Delete(id);

            return id;
        }

        public async Task<Models.VendingMachine> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            return await vendingMachinesRepository.GetById(id)
                ?? throw new NotFoundException("Такого аппарата нет");
        }

        public async Task<IEnumerable<Models.VendingMachine>> GetAll()
        {
            return await vendingMachinesRepository.GetAll()
                ?? throw new NotFoundException("Аппараты отсутствуют");
        }
    }
}
