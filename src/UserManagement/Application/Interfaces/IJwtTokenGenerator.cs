
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateTokenForgotPassword(User user);
        string ValidateTokenForgotPassword(string token);
        Task<string> GeneratePassword();
    }
}