using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.VendingMachine
{
    public record AddVendingMachineRequest(
        [Required] string TitleVendingMachine,
        decimal? AmountMoneyVendingMachine = -1);
}
