using VendingMachine.Core.Models.Enum;

namespace VendingMachine.API.Dto.Users
{
    public record GetUserResponse(
        Guid Id,
        string Name,
        decimal AmountMoney,
        UserRole Role);
}
