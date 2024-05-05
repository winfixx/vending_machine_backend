using System.ComponentModel.DataAnnotations;
using VendingMachine.Core.Models;

namespace VendingMachine.DataAccess.Entites
{
    public class VendingMachineEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public decimal AmountMoney { get; set; }
        public List<DrinkEntity>? Drinks { get; } = [];
    }
}