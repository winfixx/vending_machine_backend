using VendingMachine.Core.Models;

namespace VendingMachine.Core.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        string? GetIdFromToken(string token);
    }
}