using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VendingMachine.Core.Models.Enum;

namespace VendingMachine.DataAccess.Entites
{
    [Index(nameof(Name), IsUnique = true)]
    public class UserEntity
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public decimal AmountMoney { get; set; }
        [Required]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
