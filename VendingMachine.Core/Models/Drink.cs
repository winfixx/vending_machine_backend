namespace VendingMachine.Core.Models
{
    public class Drink(Guid id, decimal price, string title, string image, int count)
    {
        public Guid Id { get; } = id;
        public decimal Price { get; } = price;
        public string Title { get; } = title;
        public string Image { get; } = image;
        public int Count { get; } = count;
    }
}