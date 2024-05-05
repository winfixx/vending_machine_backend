using VendingMachine.Core.Models;

namespace VendingMachine.API.Dto.VendingMachine
{
    public record GetVendingMachineResponse(
        Guid Id,
        string Title,
        decimal AmountMoney,
        IEnumerable<Drink>? Drinks);
}
