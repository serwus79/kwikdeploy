using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Queries.UserUniqueNameQuery;

public class UserUniqueUserNameQuery : IRequest<bool>
{
    public string UserName { get; set; } = default!;

    public string? Id { get; set; }
}

public class UserUniqueUserNameQueryHandler : IRequestHandler<UserUniqueUserNameQuery, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserUniqueUserNameQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UserUniqueUserNameQuery request, CancellationToken cancellationToken)
    {
        string normalizedUserName = _userManager.NormalizeName(request.UserName);

        return !await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUserName && x.Id != request.Id,
            cancellationToken);
    }
}