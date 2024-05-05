using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.Drinks
{
    public record UpdateDrinkRequest(
        [Required] Guid DrinkId,
        string? TitleDrink,
        IFormFile? ImageDrink,
        int? CountDrink = -1,
        decimal? PriceDrink = -1);
}
