using VendingMachine.Core.Models.Enum;

namespace VendingMachine.Core.Models
{
    public class User(Guid id, string name, string passwordHash, decimal amountMoney, UserRole role)
    {
        public Guid Id { get; } = id;
        public string Name { get; } = name;
        public string PasswordHash { get; } = passwordHash;
        public decimal AmountMoney { get; } = amountMoney;
        public UserRole Role { get; } = role;
    }
}
