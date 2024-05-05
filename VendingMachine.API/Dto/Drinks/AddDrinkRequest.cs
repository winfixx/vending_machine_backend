using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.Drinks
{
    public record AddDrinkRequest(
        [Required] decimal PriceDrink,
        [Required] string TitleDrink,
        [Required] int CountDrink,
        [Required] IFormFile ImageDrink,
        [Required] Guid VendingMachineId);
}
