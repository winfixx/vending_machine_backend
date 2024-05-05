using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.Users
{
    public record LoginUserRequest(
        [Required] string UserName,
        [Required] string Password);
}
