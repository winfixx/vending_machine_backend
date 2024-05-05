using System.ComponentModel.DataAnnotations;

namespace VendingMachine.API.Dto.Users
{
    public record RegisterUsersRequest(
        [Required] string UserName,
        [Required] string Password);
}
