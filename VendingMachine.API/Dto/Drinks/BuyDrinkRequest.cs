using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.Drinks
{
    public record BuyDrinkRequest(
        [Required] Guid UserId,
        [Required] Guid VendingMachineId,
        [Required] decimal AmountDeposited,
        [Required] IEnumerable<BuyCartDrinkRequest> DrinksInOrder);
}
