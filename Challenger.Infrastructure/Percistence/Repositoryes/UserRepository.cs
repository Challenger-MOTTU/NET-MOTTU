using System.Net.Mail;
using Challenger.Application.DTOs.Requests;
using Challenger.Application.pagination;
using Challenger.Domain.Entities;
using Challenger.Domain.Interfaces;
using Challenger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Challenger.Infrastructure.Percistence.Repositoryes;

public class UserRepository: Repository<User>, IUserRepository
{   
    private readonly CGContext _context;
    private IUserRepository _userRepositoryImplementation;
    
    public UserRepository(CGContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        
        var user = _context.Users
            .AsEnumerable()
            .FirstOrDefault(u => u.Email.Valor == email);
        
        return user;
    }
    
    public async Task<PaginatedResult<UserSummary>> GetPageAsync(PageRequest page, UserQuery? filter = null, CancellationToken ct = default)
    {
        page.EnsureValid();

        IQueryable<User> query = _context.Users.AsNoTracking();
        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                var email = filter.Email.Trim();
                query = query.Where(u => u.Email.Valor.Contains(email));
            }

            if (filter.FromCreatedAtUc is not null)
                query = query.Where(u => u.CreatedAt >= filter.FromCreatedAtUc);
            query = filter.DescendingByCreatedAt
                ? query.OrderByDescending(u => u.CreatedAt)
                : query.OrderBy(u => u.CreatedAt);
        }
        else
        {
            query.OrderByDescending(u => u.CreatedAt);
        }
        
        var totalCount = await query.CountAsync(cancellationToken: ct);
        
        var items = await query
            .Skip(page.Offset)
            .Take(page.PageSize)
            .ToListAsync(ct);
        List<UserSummary> summaries = [];

        summaries.AddRange(items.Select(item => new UserSummary
        {
            Id = item.Id, Username = item.Username, Email = item.Email.Valor, Senha = item.Senha.Valor
        }));
        
        return new PaginatedResult<UserSummary>(summaries, totalCount, page.Page, page.PageSize);
    }
}