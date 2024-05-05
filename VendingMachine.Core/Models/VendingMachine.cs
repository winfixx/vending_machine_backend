namespace VendingMachine.Core.Models
{
    public class VendingMachine(Guid id, string title, decimal amountMoney)
    {
        public Guid Id { get; } = id;
        public string Title { get; } = title;
        public decimal AmountMoney { get; } = amountMoney;
    }
}