using Challenger.Application.DTOs.Requests;
using Challenger.Application.pagination;
using Challenger.Domain.Entities;

namespace Challenger.Domain.Interfaces;

public interface IUserRepository:IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<PaginatedResult<UserSummary>> GetPageAsync(
        PageRequest page, 
        UserQuery? filter = null, 
        CancellationToken ct = default
    );
}