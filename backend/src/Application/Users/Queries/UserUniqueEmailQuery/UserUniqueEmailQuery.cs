using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Queries.UserUniqueEmailQuery;

public class UserUniqueEmailQuery : IRequest<bool>
{
    public string Email { get; set; } = default!;
    public string? Id { get; set; }
}

public class UserUniqueEmailQueryHandler : IRequestHandler<UserUniqueEmailQuery, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserUniqueEmailQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UserUniqueEmailQuery request, CancellationToken cancellationToken)
    {
        string normalizedEmail = _userManager.NormalizeEmail(request.Email);


        return !await _userManager.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail && x.Id != request.Id,
            cancellationToken);
    }
}