using VendingMachine.Core.Exceptions;
using VendingMachine.Core.Interfaces.Repositories;
using VendingMachine.Core.Models;

namespace VendingMachine.Core.Services
{
    public class DrinksService(
        IDrinksRepository drinksRepository,
        VendingMachinesService vendingMachinesService,
        UsersService usersService,
        ImageService imageService)
    {
        private readonly IDrinksRepository drinksRepository = drinksRepository;
        private readonly VendingMachinesService vendingMachinesService = vendingMachinesService;
        private readonly UsersService usersService = usersService;
        private readonly ImageService imageService = imageService;

        public async Task<string> Buy(
            Guid userId,
            Guid vendingMachineId,
            decimal amountDeposited,
            IEnumerable<Drink> drinksInOrder)
        {
            if (userId == Guid.Empty || drinksInOrder == null)
                throw new ArgumentException("Недостаток данных");

            var user = await usersService.GetById(userId);

            if (user.AmountMoney < amountDeposited)
                throw new Exception("У пользователя недостаточно средств");

            decimal totalPrice = 0;
            foreach (var drinkInOrder in drinksInOrder)
            {
                var drink = await GetById(drinkInOrder.Id);

                if (drink.Count < drinkInOrder.Count)
                    throw new Exception($"В автомате нет столько напитков ({drinkInOrder.Title})");

                totalPrice += drink.Price * drinkInOrder.Count;
            }

            if (amountDeposited < totalPrice)
                throw new Exception("Внесенная сумма денег меньше требуемой");

            decimal change = amountDeposited - totalPrice;

            var vendingMachine = await vendingMachinesService.GetById(vendingMachineId);

            if (change > 0 && vendingMachine.AmountMoney < change)
                throw new Exception("У автомата недостаточно средств для сдачи");

            await vendingMachinesService.Update(
                vendingMachine.Title,
                vendingMachine.AmountMoney + totalPrice,
                vendingMachine);

            foreach (var drinkInOrder in drinksInOrder)
            {
                var drink = await GetById(drinkInOrder.Id);

                await Update(
                    drink.Title,
                    drink.Price,
                    null,
                    drink.Count - drinkInOrder.Count,
                    drink);
            }

            await usersService.Update(
                user.Name,
                user.PasswordHash,
                user.AmountMoney - totalPrice,
                user.Role,
                user);

            return $"{change}";
        }

        public async Task<Guid> Add(
            decimal price,
            string title,
            Stream image,
            int count,
            Guid vendingMachineId)
        {
            if (vendingMachineId == Guid.Empty || image == null) throw new ArgumentException("Недостаток данных");
            if (price < 0) throw new Exception("Цена не может быть меньше нуля");
            if (string.IsNullOrEmpty(title)) throw new Exception("Название не должно быть пустым");
            if (count < 0) throw new Exception("Кол-во не может быть меньше нуля");

            _ = await vendingMachinesService.GetById(vendingMachineId)
                ?? throw new NotFoundException("Такого аппарата нет");

            var drinkId = Guid.NewGuid();

            string imagePath = await imageService.Save(image, $"{drinkId}");

            var drink = new Drink(drinkId, price, title, imagePath, count);

            await drinksRepository.Add(drink, vendingMachineId);

            return drink.Id;
        }

        public async Task<Guid> Update(
            Guid id,
            string? title,
            decimal? price,
            Stream? image,
            int? count)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            var drink = await GetById(id);

            await Update(title, price, image, count, drink);

            return drink.Id;
        }

        public async Task<Guid> Update(
            string? title,
            decimal? price,
            Stream? image,
            int? count,
            Drink drink)
        {
            if (price < -1) throw new Exception("Цена не может быть меньше нуля");
            if (count < -1) throw new Exception("Кол-во не может быть меньше нуля");

            string imagePath = image == null || image.Length == 0
                ? drink.Image
                : await imageService.Save(image, $"{drink.Id}");

            title = string.IsNullOrEmpty(title) ? drink.Title : title;
            price = price == -1 ? drink.Price : price;
            count = count == -1 ? drink.Count : count;

            await drinksRepository.Update(new(
                drink.Id,
                price!.Value,
                title,
                imagePath,
                count!.Value));

            return drink.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            var drink = await GetById(id);

            await drinksRepository.Delete(id);
            await imageService.Delete(drink.Image);

            return id;
        }

        public async Task<Drink> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            return await drinksRepository.GetById(id)
                 ?? throw new NotFoundException("Такого напитка нет"); ;
        }

        public async Task<IEnumerable<Drink>> GetAllByVendingId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            return await drinksRepository.GetAllByVendingId(id)
                 ?? throw new NotFoundException("Напитков нет");
        }

        public async Task<IEnumerable<Drink>> GetAll()
        {
            var drinks = await drinksRepository.GetAll()
                ?? throw new NotFoundException("Напитки отсутствуют");

            return drinks ?? [];
        }
    }
}
