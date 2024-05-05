using System.ComponentModel.DataAnnotations;
using VendingMachine.Core.Models.Enum;

namespace VendingMachine.API.Dto.Users
{
    public record UpdateUserRequest(
        [Required] Guid UserId,
        string? Name,
        string? Password,
        UserRole? Role,
        decimal? AmountMoney = -1);
}
