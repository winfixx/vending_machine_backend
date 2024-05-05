using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.Drinks
{
    public record BuyCartDrinkRequest(
        [Required] Guid Id,
        [Required] decimal Price,
        [Required] string Title,
        [Required] int Count,
        [Required] int CountInCart);
}
