using Challenger.Domain.Entities;

namespace Challenger.Application.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}