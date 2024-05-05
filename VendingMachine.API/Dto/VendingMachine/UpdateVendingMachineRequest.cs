using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.VendingMachine
{
    public record UpdateVendingMachineRequest(
        [Required] Guid IdVendingMachine,
        string? TitleVendingMachine,
        decimal? AmountMoneyVendingMachine = -1);
}
