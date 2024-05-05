using VendingMachine.Core.Exceptions;
using VendingMachine.Core.Interfaces.Auth;
using VendingMachine.Core.Interfaces.Repositories;
using VendingMachine.Core.Models;
using VendingMachine.Core.Models.Enum;

namespace VendingMachine.Core.Services
{
    public class UsersService(
        IUsersRepository usersRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        private readonly IUsersRepository usersRepository = usersRepository;
        private readonly IPasswordHasher passwordHasher = passwordHasher;
        private readonly IJwtProvider jwtProvider = jwtProvider;

        public async Task<(User, string)> Registration(string userName, string password)
        {
            if (userName == null ||
                userName == string.Empty ||
                password == null) throw new ArgumentNullException("Недостаток данных");

            var candidate = await usersRepository.GetByName(userName);
            if (candidate != null)
                throw new Exception("Такой пользователь уже существует");

            string hashedPassword = passwordHasher.Generate(password);

            var user = new User(Guid.NewGuid(), userName, hashedPassword, 100, UserRole.User);

            await usersRepository.Add(user);

            string token = jwtProvider.GenerateToken(user);

            return (user, token);
        }

        public async Task<(User, string)> Login(string userName, string password)
        {
            if (userName == null || password == null)
                throw new ArgumentNullException("Недостаток данных");

            var user = await GetByName(userName);

            bool verify = passwordHasher.Verify(password, user.PasswordHash);
            if (verify == false)
                throw new Exception("Пароль неверный");

            string token = jwtProvider.GenerateToken(user);

            return (user, token);
        }

        public async Task<(User, string)> Refresh(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("Недостаток данных");

            var userId = jwtProvider.GetIdFromToken(token) 
                ?? throw new Exception("Не авторизован");

            var user = await GetById(Guid.Parse(userId));

            var newToken = jwtProvider.GenerateToken(user);

            return (user, newToken);
        }

        public async Task<User> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            return await usersRepository.GetById(id)
                ?? throw new NotFoundException("Пользователь не найден");
        }

        public async Task<User> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Недостаток данных");

            return await usersRepository.GetByName(name)
                ?? throw new NotFoundException("Пользователь не найден");
        }

        public async Task<Guid> Update(
            Guid id,
            string? name,
            string? passwordHash,
            decimal? amountMoney,
            UserRole? userRole)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Недостаток данных");

            var user = await GetById(id);

            await Update(name, passwordHash, amountMoney, userRole, user);

            return user.Id;
        }

        public async Task<Guid> Update(
            string? name,
            string? passwordHash,
            decimal? amountMoney,
            UserRole? userRole,
            User user)
        {
            name = string.IsNullOrEmpty(name) ? user.Name : name;
            passwordHash = string.IsNullOrEmpty(passwordHash) ? user.PasswordHash : passwordHash;
            amountMoney = amountMoney == -1 ? user.AmountMoney : amountMoney;
            userRole = userRole.HasValue ? userRole : user.Role;

            await usersRepository.Update(new(
                user.Id,
                name,
                passwordHash,
                amountMoney.Value,
                userRole.Value));

            return user.Id;
        }
    }
}
