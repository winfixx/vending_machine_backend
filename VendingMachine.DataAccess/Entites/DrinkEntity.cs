using System.ComponentModel.DataAnnotations;

namespace VendingMachine.DataAccess.Entites
{
    public class DrinkEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public int Count { get; set; }
        [Required]
        public Guid? VendingMachineId { get; set; }
        public VendingMachineEntity? VendingMachine { get; set; }
    }
}